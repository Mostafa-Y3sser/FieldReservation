using Microsoft.AspNetCore.Identity;

namespace FieldReservation.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumeral { get; set; } = string.Empty;

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}