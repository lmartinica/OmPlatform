using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products?> GetById(Guid id);
        Task<Products> Create(Products product);
        Task<Products?> Update(Products product);
        Task Delete(Guid id);
    }
}
