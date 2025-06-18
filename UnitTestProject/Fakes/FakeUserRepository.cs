using OmPlatform.Interfaces;
using OmPlatform.Models;

namespace OmPlatformTest.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        public List<Users> List { get; set; } = new();

        public Task<Users> Create(Users user)
        {
            user.Id = Guid.NewGuid();
            List.Add(user);
            return Task.FromResult(user);
        }

        public Task Delete(Users user)
        {
            List.Remove(user);
            return Task.CompletedTask;
        }

        public Task<Users?> GetByEmail(string email)
        {
            var entity = List.FirstOrDefault(o => o.Email == email);
            return Task.FromResult(entity);
        }

        public Task<Users?> GetByEmailAndPassword(string email, string password)
        {
            var entity = List.FirstOrDefault(o => o.Email == email);
            return Task.FromResult(entity);
        }

        public Task<Users?> GetById(Guid id)
        {
            var entity = List.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<Users>> GetList()
        {
            return Task.FromResult<IEnumerable<Users>>(List);
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }
    }
}
