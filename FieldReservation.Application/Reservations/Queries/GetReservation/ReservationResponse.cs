
namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public record ReservationResponse(
        Guid ReservationId,
        string UserId,
        DateTime StartTime,
        DateTime EndTime,
        string Status);
}