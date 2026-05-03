
namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public record OccupiedPeriodResponse(
     string PlayerName,
     DateTime StartTime,
     DateTime EndTime,
     string status);
}