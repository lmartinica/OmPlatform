using OmPlatform.DTOs.User;

namespace OmPlatform.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAll();
        Task<GetUserDto?> GetById(Guid id);
        Task<GetUserDto?> GetByEmailAndPassword(string email, string password);
        Task<GetUserDto?> GetByEmail(string email);
        Task<GetUserDto> Create(CreateUserDto userDto);
        Task<GetUserDto?> Update(Guid id, UpdateUserDto userDto);
        Task<bool> Delete(Guid id);
    }
}