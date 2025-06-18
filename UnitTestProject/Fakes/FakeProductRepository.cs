using OmPlatform.Interfaces;
using OmPlatform.Models;

namespace OmPlatformTest.Fakes
{
    public class FakeProductRepository : IProductRepository
    {
        public List<Products> List { get; set; } = new();

        public Task<Products> Create(Products product)
        {
            product.Id = Guid.NewGuid();
            List.Add(product);
            return Task.FromResult(product);
        }

        public Task Delete(Products product)
        {
            List.Remove(product);
            return Task.CompletedTask;
        }

        public Task<Products?> GetById(Guid id)
        {
            var entity = List.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<Products>> GetList()
        {
            return Task.FromResult<IEnumerable<Products>>(List);
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}
