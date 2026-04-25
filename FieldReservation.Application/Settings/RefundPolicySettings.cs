namespace FieldReservation.Application.Settings
{
    /// <summary>Configurable refund policy with ordered tiers based on hours before reservation start.</summary>
    public class RefundPolicySettings
    {
        /// <summary>
        /// Ordered list of refund tiers. At runtime the handler sorts these descending by MinHoursBefore,
        /// so the order in appsettings does not matter — the highest threshold wins first.
        /// </summary>
        public List<RefundTier> Tiers { get; set; } = [];
    }

    /// <summary>A single tier in the refund policy.</summary>
    public class RefundTier
    {
        /// <summary>
        /// Minimum hours before the reservation start time required for this refund percentage to apply.
        /// For example, MinHoursBefore = 12 means the user must cancel at least 12 hours before the booking.
        /// </summary>
        public int MinHoursBefore { get; set; }

        /// <summary>Percentage of the total charge to refund. Range: 0–100.</summary>
        public int RefundPercentage { get; set; }
    }
}
