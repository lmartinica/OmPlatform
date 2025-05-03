using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OmPlatform.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly ClaimsPrincipal _user;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        }

        public Guid GetUserId()
        {
            var userIdString = _user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                new Exception("Invalid user ID in token.");
            return userId;
        }

        public bool IsUser()
        {
            return _user.IsInRole("User");
        }
        public bool IsAllowed(Guid userId)
        {
            if(IsAdmin()) return true;
            return GetUserId() == userId;
        }

        public bool IsAdmin()
        {
            return _user.IsInRole("Admin");
        }
    }
}
