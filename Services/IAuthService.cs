using OmPlatform.DTOs.User;

namespace OmPlatform.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(GetUserDto user);
    }
}
