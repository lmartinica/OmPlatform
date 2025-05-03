using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbAppContext _context;

        public UserRepository(DbAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetList()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Users?> GetByEmailAndPassword(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email == email);
            if (user == null) return null;

            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<Users> Create(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Users user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
