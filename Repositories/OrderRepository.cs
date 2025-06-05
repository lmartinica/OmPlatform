using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbAppContext _context;

        public OrderRepository(DbAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetList()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Orders?> GetById(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Orders> Create(Orders order)
        {
            // UnitOfWork method 
            _context.Orders.Add(order);
            return order;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Orders order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
