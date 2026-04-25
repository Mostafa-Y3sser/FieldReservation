namespace FieldReservation.Application.Auth.Dtos;

public record UserDto(
    string Id,
    string FullName,
    string Email,
    string? PhoneNumber
);
