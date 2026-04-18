using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using FieldReservation.Application.Interfaces;
using FieldReservation.Infrastructure.Settings;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Domain.Entities;

namespace FieldReservation.Infrastructure.Services
{
    public class TokenService(
         UserManager<ApplicationUser> _userManager,
         IOptions<JwtSettings> _options,
         IAppDbContext applicationDbContext) : ITokenService
    {
        public async Task<string> CreateAccessTokenAsync(ApplicationUser user)
        {
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();
            foreach (string role in roles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            JwtSettings jwtSettings = _options.Value;


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshTokenAsync(string userId)
        {
            byte[] randomBytes = new byte[64];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomBytes);

            RefreshToken refreshToken = new RefreshToken()
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(_options.Value.RefreshTokenExpirationDays)
            };

            applicationDbContext.RefreshTokens.Add(refreshToken);
            await applicationDbContext.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}