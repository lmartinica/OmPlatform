using OmPlatform.DTOs.Order;

namespace OmPlatform.Interfaces
{
    public interface ICurrentUserService
    {
        Guid GetUserId();
        bool IsUser();
        bool IsAllowed(Guid userId);
        public bool IsAdmin();
    }
}
