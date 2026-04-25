namespace FieldReservation.Application.Admin.Queries.GetAllUsers;

public record UserResponse(
    string Id,
    string FullName,
    string Email,
    string? PhoneNumber
);
