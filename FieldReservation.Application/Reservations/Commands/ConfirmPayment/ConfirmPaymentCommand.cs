using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.ConfirmPayment
{
    /// <summary>
    /// Dispatched by the Stripe webhook handler when a checkout.session.completed event is received.
    /// </summary>
    /// <param name="StripeSessionId">The Stripe Session ID to look up the reservation.</param>
    /// <param name="PaymentIntentId">The Stripe PaymentIntent ID, stored for future refund operations.</param>
    public record ConfirmPaymentCommand(
        string StripeSessionId,
        string PaymentIntentId) : IRequest<Result>;
}
