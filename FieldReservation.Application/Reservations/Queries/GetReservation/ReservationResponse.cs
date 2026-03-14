
namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public record ReservationResponse(
        Guid Id,
        Guid FieldId,
        Guid UserId,
        DateTime StartTime,
        DateTime EndTime);
}