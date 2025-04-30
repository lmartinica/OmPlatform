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
            return await _context.Orders.ToListAsync();
        }

        public async Task<Orders?> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Orders> Create(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Orders?> Update(Orders order)
        {
            var orderFound = await _context.Products.FindAsync(order.Id);
            if (orderFound != null)
            {
                _context.Entry(orderFound).State = EntityState.Detached;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return order;
            }
            return null;
        }

        public async Task Delete(Orders order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
