namespace FieldReservation.Application.Interfaces
{
    /// <summary>Abstraction over the Stripe payment provider.</summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a Stripe Checkout Session for the given reservation and returns the hosted checkout URL.
        /// </summary>
        /// <param name="reservationId">The reservation to link to the session via metadata.</param>
        /// <param name="amountInCents">Total charge amount in the smallest currency unit (e.g. 100 = $1.00).</param>
        /// <param name="currency">ISO 4217 currency code, e.g. "egp" or "usd".</param>
        /// <param name="successUrl">URL Stripe redirects to after successful payment.</param>
        /// <param name="cancelUrl">URL Stripe redirects to if the user aborts checkout.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
        /// <returns>A tuple containing the Stripe Session ID and the hosted checkout URL.</returns>
        Task<(string SessionId, string CheckoutUrl)> CreateCheckoutSessionAsync(
            Guid reservationId,
            long amountInCents,
            string currency,
            string successUrl,
            string cancelUrl,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Issues a full refund against a completed Stripe PaymentIntent.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent ID stored on the reservation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
        Task RefundFullAsync(
            string paymentIntentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Issues a partial refund by percentage against a completed Stripe PaymentIntent.
        /// Stripe fetches the original charge amount to compute the refund value.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent ID stored on the reservation.</param>
        /// <param name="percentage">Refund percentage (1–99).</param>
        /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
        Task RefundPartialAsync(
            string paymentIntentId,
            int percentage,
            CancellationToken cancellationToken = default);
    }
}

