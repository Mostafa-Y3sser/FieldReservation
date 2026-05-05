
namespace FieldReservation.Application.Reservations.Queries.GetMyReservations;

public record MyReservationResponse(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    string Status
);