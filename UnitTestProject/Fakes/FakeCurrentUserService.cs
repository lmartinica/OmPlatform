using OmPlatform.Interfaces;

namespace OmPlatformTest.Fakes
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; set; }
        public bool IsAdm {  get; set; }

        public FakeCurrentUserService(Guid userId, bool isAdmin) 
        {
            UserId = userId;
            IsAdm = isAdmin;
        }

        public Guid GetUserId()
        {
            return UserId;
        }

        public bool IsAdmin()
        {
            return IsAdm;
        }

        public bool IsAllowed(Guid userId)
        {
            if (IsAdmin()) return true;
            return GetUserId() == userId;
        }

        public bool IsUser()
        {
            return !IsAdm;
        }
    }
}
