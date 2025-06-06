using OmPlatform.Core;
using OmPlatform.DTOs.User;

namespace OmPlatform.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<GetUserDto>>> GetList();
        Task<Result<GetUserDto>> GetById(Guid id);
        Task<Result<GetUserDto>> GetByEmailAndPassword(string email, string password);
        Task<Result<GetUserDto>> GetByEmail(string email);
        Task<Result<GetUserDto>> Create(CreateUserDto userDto);
        Task<Result<GetUserDto>> Update(Guid id, UpdateUserDto userDto);
        Task<Result<bool>> Delete(Guid id);
    }
}