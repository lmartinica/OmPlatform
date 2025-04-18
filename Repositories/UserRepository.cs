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

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> Create(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users?> Update(Users user)
        {
            var userFound = await _context.Users.FindAsync(user.Id);
            if (userFound != null)
            {
                _context.Entry(userFound).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
