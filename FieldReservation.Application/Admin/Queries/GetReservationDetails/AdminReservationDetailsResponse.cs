namespace FieldReservation.Application.Admin.Queries.GetReservationDetails;

public record AdminReservationDetailsResponse(
    Guid ReservationId,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    string UserId,
    string UserFullName,
    string UserEmail,
    string? UserPhoneNumber,
    string? MaintenanceNote
);