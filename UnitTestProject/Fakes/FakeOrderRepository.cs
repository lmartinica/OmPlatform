using OmPlatform.Interfaces;
using OmPlatform.Models;

namespace OmPlatformTest.Fakes
{
    public class FakeOrderRepository : IOrderRepository
    {
        public List<Orders> List { get; set; } = new();

        public Task<Orders> Create(Orders order)
        {
            order.Id = Guid.NewGuid();
            List.Add(order);
            return Task.FromResult(order);
        }

        public Task Delete(Orders order)
        {
            List.Remove(order);
            return Task.CompletedTask;
        }

        public Task<Orders?> GetById(Guid id)
        {
            var entity = List.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<Orders>> GetList()
        {
            return Task.FromResult<IEnumerable<Orders>>(List);
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }
    }
}
