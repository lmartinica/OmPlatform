using OmPlatform.DTOs.User;

namespace OmPlatform.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAll();
        Task<GetUserDto?> GetById(Guid id);
        Task<GetUserDto> Create(CreateUserDto userDto);
        Task<GetUserDto?> Update(Guid id, UpdateUserDto userDto);
        Task Delete(Guid id);
    }
}