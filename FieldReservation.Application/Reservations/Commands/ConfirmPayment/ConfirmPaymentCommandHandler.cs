using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.ConfirmPayment
{
    public sealed class ConfirmPaymentCommandHandler(IAppDbContext context)
        : IRequestHandler<ConfirmPaymentCommand, Result>
    {
        public async Task<Result> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.StripeSessionId == request.StripeSessionId, cancellationToken);

            if (reservation is null)
                return Error.NotFound(description: $"No reservation found for Stripe session '{request.StripeSessionId}'.");

            // Store the PaymentIntent ID now so future refunds can reference it
            reservation.SetStripePaymentIntentId(request.PaymentIntentId);

            try
            {
                reservation.ConfirmPayment();
            }
            catch (InvalidOperationException)
            {
                // Idempotency: if already Confirmed (e.g. duplicate webhook delivery), just return OK
                return Result.Ok();
            }

            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
