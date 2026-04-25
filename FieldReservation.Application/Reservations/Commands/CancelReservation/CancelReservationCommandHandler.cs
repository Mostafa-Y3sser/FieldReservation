using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Interfaces;
using FieldReservation.Domain.Enums;
using FieldReservation.Application.Settings;

namespace FieldReservation.Application.Reservations.Commands.CancelReservation
{
    public sealed class CancelReservationCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPaymentService paymentService,
        IOptions<RefundPolicySettings> refundPolicyOptions)
        : IRequestHandler<CancelReservationCommand, Result>
    {
        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.Id && r.UserId == userId, cancellationToken);

            if (reservation is null)
                return Error.NotFound(description: "The reservation was not found.");

            if (reservation.Status == ReservationStatus.Cancelled)
                return Error.Failure(description: "The reservation is already cancelled.");

            if (reservation.Status == ReservationStatus.Maintenance)
                return Error.Failure(description: "Maintenance blocks cannot be cancelled through this endpoint.");

            // Attempt refund only for paid (Confirmed) reservations that have a PaymentIntent
            if (reservation.Status == ReservationStatus.Confirmed
                && reservation.StripePaymentIntentId is not null)
            {
                var hoursBeforeStart = (reservation.StartTime - DateTime.UtcNow).TotalHours;
                var refundPercentage = CalculateRefundPercentage(hoursBeforeStart, refundPolicyOptions.Value);

                if (refundPercentage == 100)
                {
                    await paymentService.RefundFullAsync(
                        reservation.StripePaymentIntentId,
                        cancellationToken);
                }
                else if (refundPercentage > 0)
                {
                    await paymentService.RefundPartialAsync(
                        reservation.StripePaymentIntentId,
                        refundPercentage,
                        cancellationToken);
                }
                // refundPercentage == 0 → no refund, just cancel
            }

            reservation.Cancel();
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }

        /// <summary>
        /// Returns the refund percentage based on how many hours remain before the reservation starts.
        /// Tiers are evaluated highest-threshold-first; the first tier where hoursBeforeStart >= MinHoursBefore wins.
        /// </summary>
        private static int CalculateRefundPercentage(double hoursBeforeStart, RefundPolicySettings policy)
        {
            var orderedTiers = policy.Tiers
                .OrderByDescending(t => t.MinHoursBefore)
                .ToList();

            foreach (var tier in orderedTiers)
            {
                if (hoursBeforeStart >= tier.MinHoursBefore)
                    return tier.RefundPercentage;
            }

            return 0;
        }
    }
}