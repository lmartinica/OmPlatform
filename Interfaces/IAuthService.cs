using OmPlatform.DTOs.User;

namespace OmPlatform.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(GetUserDto user);
    }
}
