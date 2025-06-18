using OmPlatform.Models;

namespace OmPlatform.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetList();
        Task<Users?> GetById(Guid id);
        Task<Users?> GetByEmailAndPassword(string email, string password);
        Task<Users?> GetByEmail(string email);
        Task<Users> Create(Users user);
        Task Update();
        Task Delete(Users user);
    }
}
