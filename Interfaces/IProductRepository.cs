using OmPlatform.Models;

namespace OmPlatform.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetList();
        Task<Products?> GetById(Guid id);
        Task<Products> Create(Products product);
        Task Delete(Products product);
        Task SaveAsync();
    }
}
