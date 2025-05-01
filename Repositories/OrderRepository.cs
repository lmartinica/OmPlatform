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

        public async Task<IEnumerable<Orders>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Orders?> GetById(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Orders> Create(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
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
