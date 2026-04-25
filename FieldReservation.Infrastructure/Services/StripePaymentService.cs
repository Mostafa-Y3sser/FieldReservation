using Stripe;
using FieldReservation.Application.Interfaces;
using FieldReservation.Application.Settings;

namespace FieldReservation.Infrastructure.Services
{
    /// <summary>
    /// Stripe implementation of <see cref="IPaymentService"/>.
    /// Uses Stripe Checkout Sessions for payment collection and the Refunds API for cancellations.
    /// The Stripe API key is set globally via StripeConfiguration.ApiKey in ServiceRegistration.
    /// </summary>
    public sealed class StripePaymentService : IPaymentService
    {
        /// <inheritdoc/>
        public async Task<(string SessionId, string CheckoutUrl)> CreateCheckoutSessionAsync(
            Guid reservationId,
            long amountInCents,
            string currency,
            string successUrl,
            string cancelUrl,
            CancellationToken cancellationToken = default)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                Mode = "payment",
                Currency = currency,
                LineItems =
                [
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = amountInCents,
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Field Reservation",
                                Description = $"Reservation ID: {reservationId}"
                            }
                        },
                        Quantity = 1
                    }
                ],
                // Stripe replaces {CHECKOUT_SESSION_ID} with the actual session ID at redirect time
                SuccessUrl = $"{successUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = cancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    ["reservation_id"] = reservationId.ToString()
                },
                // Expire the session after 30 minutes to free the slot for other users
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

            return (session.Id, session.Url);
        }

        /// <inheritdoc/>
        public async Task RefundFullAsync(
            string paymentIntentId,
            CancellationToken cancellationToken = default)
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
                // No Amount → Stripe issues a full refund
            };

            var service = new RefundService();
            await service.CreateAsync(options, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RefundPartialAsync(
            string paymentIntentId,
            int percentage,
            CancellationToken cancellationToken = default)
        {
            if (percentage is <= 0 or >= 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Partial refund percentage must be between 1 and 99.");

            // Fetch the PaymentIntent to get the original charged amount
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

            var originalAmountInCents = paymentIntent.Amount;
            var refundAmountInCents = (long)Math.Round(originalAmountInCents * ((double)percentage / 100), MidpointRounding.AwayFromZero);

            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Amount = refundAmountInCents
            };

            var service = new RefundService();
            await service.CreateAsync(options, cancellationToken: cancellationToken);
        }
    }
}
