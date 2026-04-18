using System.Text;
using Google.Apis.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using FieldReservation.Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Interfaces;
using FieldReservation.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.Google;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Constants;

namespace FieldReservation.Infrastructure.Services
{
    public sealed class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
       IEmailService emailService, IOptions<GoogleAuthSettings> googleOptions, IAppDbContext applicationDbContext,
       IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IOptions<JwtSettings> jwtOptions) : IAuthService
    {
        public async Task<Result<AuthResponseDto>> RegisterAsync(string firstName, string lastName, string email, string phoneNumber,
            string password, string confirmPassword)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
                return Error.Validation(description: "User with this email already exist.");

            var user = new ApplicationUser
            {
                FullName = $"{firstName} {lastName}",
                UserName = email[..email.IndexOf('@')],
                Email = email,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var identityResult = await userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
                return Error.Validation(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            await userManager.AddToRoleAsync(user, Roles.Player);

            await SendEmailVerificationTokenAsync(user.Email);

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            if (!await userManager.IsEmailConfirmedAsync(user))
                return Error.Validation(description: "Email is not confirmed. Please verify your email before logging in.");

            var signInResult = await signInManager
                .CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
                return Error.InvalidCredentials(description: "Email or Password is incorrect.");

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result> SendEmailVerificationTokenAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string emailVerificationLink = $"https://Hagz.com/email-verification?email={email}&token={encodedToken}";
            string htmlBody = emailVerificationHtmlBody(emailVerificationLink);

            await emailService.SendEmailAsync(email, "Hagz - Email Verification", htmlBody);

            return Result.Ok();
        }

        public async Task<Result<AuthResponseDto>> EmailVerificationAsync(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            if (await userManager.IsEmailConfirmedAsync(user))
                return Error.Validation(description: "Email is already confirmed.");

            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            IdentityResult identityResult = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (!identityResult.Succeeded)
                return Error.Validation(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string forgotPasswordLink = $"https://Hagz.com/reset-password?email={email}&token={encodedToken}";
            string htmlBody = forgetPasswordHtmlBody(forgotPasswordLink);

            await emailService.SendEmailAsync(email, "Hagz - Reset Password", htmlBody);

            return Result.Ok();
        }

        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword, string confirmNewPassword)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            IdentityResult identityResult = await userManager.ResetPasswordAsync(user, decodedToken, newPassword);
            if (!identityResult.Succeeded)
                return Error.Failure(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            return Result.Ok();
        }

        public async Task<Result<AuthResponseDto>> GoogleSignInAsync(string idToken)
        {
            var payloadResult = await ValidateGoogleTokenAsync(idToken);
            if (payloadResult.IsFailed)
                return payloadResult.Errors.ToList();

            var payload = payloadResult.Value;

            ApplicationUser? user = await userManager.FindByLoginAsync(GoogleDefaults.AuthenticationScheme, payload.Subject);
            if (user == null)
                return Error.NotFound(description: "user not found. Please sign up first.");

            var logins = await userManager.GetLoginsAsync(user);
            if (!logins.Any(l => l.LoginProvider == GoogleDefaults.AuthenticationScheme))
                return Error.Validation(description: "This account is not registered using Google.");

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponseDto>> GoogleSignUpAsync(string idToken)
        {
            var payloadResult = await ValidateGoogleTokenAsync(idToken);
            if (payloadResult.IsFailed)
                return payloadResult.Errors.ToList();

            var payload = payloadResult.Value;

            var uxistingUser = await userManager.FindByEmailAsync(payload.Email);
            if (uxistingUser != null)
                return Error.Validation(description: "user with this email already exists, Please sign in instead.");

            ApplicationUser newUser = new()
            {
                UserName = payload.Email[..payload.Email.IndexOf('@')],
                Email = payload.Email,
                EmailConfirmed = true,
                FullName = payload.Name,
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult result = await userManager.CreateAsync(newUser);
            if (!result.Succeeded)
                return Error.Failure(description: "Failed to create user account.");

            var loginResult = await userManager.AddLoginAsync(
                newUser,
                new UserLoginInfo(GoogleDefaults.AuthenticationScheme, payload.Subject, GoogleDefaults.AuthenticationScheme));

            if (!loginResult.Succeeded)
            {
                await userManager.DeleteAsync(newUser);
                return Error.Failure(description: "Failed to link Google account.");
            }

            return await generateAuthResponseAsync(newUser);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var existingRefreshToken = applicationDbContext.RefreshTokens
                .Where(rt => rt.Token == refreshToken).FirstOrDefault();

            if (existingRefreshToken == null)
                return Error.Validation(description: "Invalid Refresh Token.");
            if (existingRefreshToken.RevokedOn != null)
                return Error.Validation(description: "Refresh Token has been revoked.");
            if (existingRefreshToken.ExpiresOn <= DateTime.UtcNow)
                return Error.Validation(description: "Refresh Token has expired.");

            var user = await userManager.FindByIdAsync(existingRefreshToken.UserId);
            if (user == null)
                return Error.NotFound(description: "User not found.");

            existingRefreshToken.RevokedOn = DateTime.UtcNow;
            applicationDbContext.RefreshTokens.Update(existingRefreshToken);
            await applicationDbContext.SaveChangesAsync();

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existingRefreshToken = applicationDbContext.RefreshTokens
                .Where(rt => rt.Token == refreshToken).FirstOrDefault();

            if (existingRefreshToken == null)
                return Error.Validation(description: "Invalid Refresh Token.");
            if (existingRefreshToken.RevokedOn != null)
                return Error.Validation(description: "Refresh Token has been revoked.");
            if (existingRefreshToken.ExpiresOn <= DateTime.UtcNow)
                return Error.Validation(description: "Refresh Token has expired.");

            if (existingRefreshToken.UserId != (httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Error.Unauthorized();

            existingRefreshToken.RevokedOn = DateTime.UtcNow;
            applicationDbContext.RefreshTokens.Update(existingRefreshToken);
            await applicationDbContext.SaveChangesAsync();

            return Result.Ok();
        }

        // Helper Methods
        private async Task<Result<GoogleJsonWebSignature.Payload>> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var payloadResult = await GoogleJsonWebSignature.ValidateAsync(
                    idToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { googleOptions.Value.ClientId }
                    });

                return payloadResult;
            }
            catch
            {
                return Error.Unauthorized(description: "Invalid Google refreshToken provided.");
            }
        }
        private async Task<AuthResponseDto> generateAuthResponseAsync(ApplicationUser user) =>
             new AuthResponseDto
            (
                user.FullName,
                user.Email!,
                await tokenService.CreateAccessTokenAsync(user),
                await tokenService.CreateRefreshTokenAsync(user.Id),
                DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
            );
        private string emailVerificationHtmlBody(string emailVerificationLink) =>
           $@"
            <div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='https://www.bentonvillear.com/ImageRepository/Document?documentID=19609' alt='Hagz' style='width:120px;'>
                </div>
                <h2 style='color:#2c3e50; text-align:center;'>Verify Your Email</h2>
                <p style='color:#555;'>Hello,</p>
                <p style='color:#555;'>We received a request to verify your email. Click the button below to continue:</p>
                <div style='text-align:center; margin:30px 0;'>
                    <a href='{emailVerificationLink}' style='background:#3498db; color:white; padding:14px 28px; text-decoration:none; font-weight:bold; border-radius:6px; display:inline-block;'>
                        Verify Email
                    </a>
                </div>
                <p style='color:#555;'>This link will expire soon.</p>
                <p style='color:#888; font-size:12px;'>If you didn’t request this, please ignore this email.</p>
                <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
                <p style='color:#888; font-size:12px; text-align:center;'>Thanks, <br/>The Hagz Team.</p>
            </div>
            ";
        private string forgetPasswordHtmlBody(string token) =>
           $@"
            <div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='https://www.bentonvillear.com/ImageRepository/Document?documentID=19609' alt='Hagz' style='width:120px;'>
                </div>
                <h2 style='color:#2c3e50; text-align:center;'>Reset Your Password</h2>
                <p style='color:#555;'>Hello,</p>
                <p style='color:#555;'>We received a request to reset your password. Copy the code below:</p>
                <div style='text-align:center; margin:30px 0;'>
                   {token}
                </div>
                <p style='color:#555;'>This code can only be used once and expires in 10 minutes.</p>
                <p style='color:#888; font-size:12px;'>If you didn’t request this, please ignore this email.</p>
                <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
                <p style='color:#888; font-size:12px; text-align:center;'>Thanks, <br/>The Hagz Team.</p>
            </div>
            ";
    }
}