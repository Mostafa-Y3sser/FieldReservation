namespace FieldReservation.Application.Auth.Dtos;

public record UserProfileResponse(
    string FullName,
    string Email,
    string? PhoneNumber,
    string UserName
);
