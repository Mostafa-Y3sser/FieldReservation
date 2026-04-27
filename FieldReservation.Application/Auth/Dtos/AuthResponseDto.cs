
namespace FieldReservation.Application.Auth.Dtos
{
    public sealed record AuthResponseDto
       (
           string FullName,
           string RoleName,
           string Email,
           string AccessToken,
           string RefreshToken,
           DateTime AccessTokenExpiresAt
       );
}