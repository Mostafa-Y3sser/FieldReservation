using FieldReservation.Domain.Entities;

namespace FieldReservation.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(ApplicationUser user);
        Task<string> CreateRefreshTokenAsync(string userID);
    }
}