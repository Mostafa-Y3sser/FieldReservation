using System.ComponentModel.DataAnnotations;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ReservationStatus Status { get; private set; }
        public string? MaintenanceNote { get; private set; }

        /// <summary>The Stripe Checkout Session ID created when the reservation was booked.</summary>
        public string? StripeSessionId { get; private set; }

        /// <summary>
        /// The Stripe PaymentIntent ID captured after successful payment.
        /// Required for issuing refunds.
        /// </summary>
        public string? StripePaymentIntentId { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; private set; } = null!;

        public bool IsMaintenance => Status == ReservationStatus.Maintenance;

        /// <summary>Creates a new reservation in PendingPayment status awaiting Stripe payment.</summary>
        public static Reservation Create(Guid fieldId, string userId, DateTime startTime, DateTime endTime)
        {
            return new Reservation
            {
                FieldId = fieldId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.PendingPayment
            };
        }

        public static Reservation CreateMaintenance(Guid fieldId, string userId, DateTime startTime, DateTime endTime, string note)
        {
            return new Reservation
            {
                FieldId = fieldId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.Maintenance,
                MaintenanceNote = note
            };
        }

        /// <summary>Stores the Stripe Checkout Session ID after the session is created.</summary>
        public void SetStripeSessionId(string sessionId)
        {
            StripeSessionId = sessionId;
        }

        /// <summary>Stores the Stripe PaymentIntent ID after successful payment, for use in refunds.</summary>
        public void SetStripePaymentIntentId(string paymentIntentId)
        {
            StripePaymentIntentId = paymentIntentId;
        }

        /// <summary>Confirms the reservation after successful Stripe payment.</summary>
        /// <exception cref="InvalidOperationException">Thrown if the reservation is not in PendingPayment status.</exception>
        public void ConfirmPayment()
        {
            if (Status != ReservationStatus.PendingPayment)
                throw new InvalidOperationException("Only reservations in PendingPayment status can be confirmed via payment.");

            Status = ReservationStatus.Confirmed;
        }

        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
        }

        public void Reschedule(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        // Navigation Properties
        public Guid FieldId { get; private set; }
        public Field Field { get; private set; } = null!;

        public string UserId { get; private set; } = string.Empty;
        public ApplicationUser User { get; private set; } = null!;
    }
}