using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Interfaces;
using FieldReservation.Domain.Entities;
using FieldReservation.Domain.Enums;
using FieldReservation.Application.Settings;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    public sealed class CreateReservationCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPaymentService paymentService,
        IOptions<StripeSettings> stripeOptions)
        : IRequestHandler<CreateReservationCommand, Result<CreateReservationResponse>>
    {
        public async Task<Result<CreateReservationResponse>> Handle(
            CreateReservationCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            // 1. Fetch the single "Elite Turf" field
            var field = await context.Fields
                .FirstOrDefaultAsync(f => f.Name == "Elite Turf", cancellationToken);

            if (field == null)
                return Error.Failure(description: "The 'Elite Turf' field has not been initialized.");

            // 2. Check for overlaps — PendingPayment slots are not occupied, only Confirmed ones are
            var overlapExists = await context.Reservations
                .AnyAsync(r =>
                    r.FieldId == field.Id &&
                    r.Status == ReservationStatus.Confirmed &&
                    request.StartTime < r.EndTime &&
                    request.EndTime > r.StartTime,
                    cancellationToken);

            if (overlapExists)
                return Error.Conflict(description: "The requested time slot is already occupied.");

            // 3. Create and save reservation in PendingPayment state
            var reservation = Reservation.Create(
                field.Id,
                userId,
                request.StartTime,
                request.EndTime);

            context.Reservations.Add(reservation);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error.Conflict(description: "The reservation could not be completed because the slot was booked by someone else.");
            }

            // 4. Calculate amount in cents: HourlyRate × hours
            var durationHours = (decimal)(request.EndTime - request.StartTime).TotalHours;
            var totalAmount = field.HourlyRate * durationHours;
            var amountInCents = (long)Math.Round(totalAmount * 100, MidpointRounding.AwayFromZero);

            // 5. Create Stripe Checkout Session
            var stripe = stripeOptions.Value;
            var (sessionId, checkoutUrl) = await paymentService.CreateCheckoutSessionAsync(
                reservation.Id,
                amountInCents,
                currency: stripe.Currency,
                successUrl: stripe.SuccessUrl,
                cancelUrl: stripe.CancelUrl,
                cancellationToken);

            // 6. Persist the session ID on the reservation
            reservation.SetStripeSessionId(sessionId);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateReservationResponse(reservation.Id, checkoutUrl);
        }
    }
}