using OmPlatform.Core;
using OmPlatform.Interfaces;
using System.Security.Claims;

namespace OmPlatform.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal _user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
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
            return _user.IsInRole(Constants.User);
        }
        public bool IsAllowed(Guid userId)
        {
            if(IsAdmin()) return true;
            return GetUserId() == userId;
        }

        public bool IsAdmin()
        {
            return _user.IsInRole(Constants.Admin);
        }
    }
}
