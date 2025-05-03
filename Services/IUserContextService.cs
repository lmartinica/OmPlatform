using OmPlatform.DTOs.Order;

namespace OmPlatform.Services
{
    public interface IUserContextService
    {
        Guid GetUserId();
        bool IsUser();
        bool IsAllowed(Guid userId);
        public bool IsAdmin();
    }
}
