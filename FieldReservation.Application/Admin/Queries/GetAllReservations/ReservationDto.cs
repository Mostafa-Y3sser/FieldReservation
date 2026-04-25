namespace FieldReservation.Application.Admin.Queries.GetAllReservations;

public record ReservationDto(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    string? MaintenanceNote,
    string? PlayerName
);