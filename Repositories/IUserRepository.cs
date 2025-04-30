using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAll();
        Task<Users?> GetById(Guid id);
        Task<Users?> GetByEmailAndPassword(string email, string password);
        Task<Users?> GetByEmail(string email);
        Task<Users> Create(Users user);
        Task<Users?> Update(Users user);
        Task Delete(Guid id);
    }
}
