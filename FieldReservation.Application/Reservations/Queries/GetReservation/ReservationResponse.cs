
namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public record ReservationResponse(
        string PlayerName,
        string Email,
        DateTime StartTime,
        DateTime EndTime,
        string Status);
}