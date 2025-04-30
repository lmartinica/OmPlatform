using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products?> GetById(Guid id);
        Task<Products> Create(Products product);
        Task Update();
        Task Delete(Products product);
    }
}
