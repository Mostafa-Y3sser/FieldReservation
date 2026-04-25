namespace FieldReservation.Application.Settings
{
    /// <summary>Stripe API credentials and redirect URLs, bound from configuration or user secrets.</summary>
    public class StripeSettings
    {
        /// <summary>Stripe publishable key (pk_test_... or pk_live_...).</summary>
        public string PublishableKey { get; set; } = string.Empty;

        /// <summary>Stripe secret key (sk_test_... or sk_live_...). Store in user secrets, never in appsettings.</summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Webhook signing secret (whsec_...) obtained from the Stripe CLI or Dashboard.
        /// Used to verify that incoming webhook calls are genuinely from Stripe.
        /// </summary>
        public string WebhookSecret { get; set; } = string.Empty;

        /// <summary>URL Stripe redirects to after a successful payment.</summary>
        public string SuccessUrl { get; set; } = string.Empty;

        /// <summary>URL Stripe redirects to when the user aborts checkout.</summary>
        public string CancelUrl { get; set; } = string.Empty;

        /// <summary>ISO 4217 currency code for all transactions, e.g. "usd" or "egp".</summary>
        public string Currency { get; set; } = "usd";
    }
}
