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

        [Timestamp]
        public byte[] RowVersion { get; private set; } = null!;

        public bool IsMaintenance => Status == ReservationStatus.Maintenance;

        public static Reservation Create(Guid fieldId, string userId, DateTime startTime, DateTime endTime)
        {
            return new Reservation
            {
                FieldId = fieldId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.Confirmed
            };
        }

        public static Reservation CreateMaintenance(Guid fieldId, DateTime startTime, DateTime endTime, string note)
        {
            return new Reservation
            {
                FieldId = fieldId,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.Maintenance,
                MaintenanceNote = note
            };
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