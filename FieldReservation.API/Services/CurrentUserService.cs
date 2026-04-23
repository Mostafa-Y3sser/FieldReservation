using System.Security.Claims;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.API.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
        : ICurrentUserService
    {
        public string? UserId
        {
            get
            {
                var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return string.IsNullOrEmpty(userId) ? null : userId;
            }
        }
    }
}
