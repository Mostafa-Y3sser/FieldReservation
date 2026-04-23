
namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public record OccupiedPeriodResponse(
     DateTime StartTime,
     DateTime EndTime,
     bool IsMaintenance,
     string? MaintenanceNote);
}