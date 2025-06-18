using OmPlatform.Interfaces;

namespace OmPlatformTest.Fakes
{
    public class FakeOrderUnitOfWork : IOrderUnitOfWork
    {
        public IOrderRepository Orders { get; set; }
        public IProductRepository Products { get; set; }

        public FakeOrderUnitOfWork(IOrderRepository orders, IProductRepository products)
        {
            Orders = orders;
            Products = products;
        }

        public Task<int> SaveAsync()
        {
            return Task.FromResult(0);
        }
    }
}
