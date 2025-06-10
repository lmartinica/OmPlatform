using OmPlatform.Core;
using OmPlatform.Interfaces;

namespace OmPlatform.Repositories
{
    public class OrderUnitOfWork : IOrderUnitOfWork
    {
        private readonly DbAppContext _context;
        public IOrderRepository Orders { get; set; }
        public IProductRepository Products { get; set; }

        public OrderUnitOfWork(DbAppContext context, IOrderRepository orders, IProductRepository products)
        {
            _context = context;
            Orders = orders;
            Products = products;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
