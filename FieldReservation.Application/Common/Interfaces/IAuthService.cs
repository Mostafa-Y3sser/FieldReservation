using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(string firstName, string lastName, string email, string phoneNumber,
            string password, string ConfirmPassword);

        Task<Result<AuthResponseDto>> LoginAsync(string email, string password);

        Task<Result> SendEmailVerificationTokenAsync(string email);
        Task<Result<AuthResponseDto>> EmailVerificationAsync(string email, string token);
        Task<Result> ForgotPasswordAsync(string email);
        Task<Result> ResetPasswordAsync(string email, string token, string newPassword, string confirmNewPassword);
        Task<Result<AuthResponseDto>> GoogleSignInAsync(string idToken);
        Task<Result<AuthResponseDto>> GoogleSignUpAsync(string idToken);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result> RevokeRefreshTokenAsync(string refreshToken);
        Task<Result> BlockUserAsync(string userId);
        Task<Result<List<UserDto>>> GetAllUsersAsync();
    }
}