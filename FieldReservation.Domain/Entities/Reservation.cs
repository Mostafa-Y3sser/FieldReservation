namespace FieldReservation.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid FieldId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public static Reservation Create(Guid fieldId, Guid userId, DateTime startTime, DateTime endTime)
        {
            return new Reservation
            {
                FieldId = fieldId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime
            };
        }
    }
}