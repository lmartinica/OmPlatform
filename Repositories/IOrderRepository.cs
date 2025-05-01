using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAll();
        Task<Orders?> GetById(Guid id);
        Task<Orders> Create(Orders order);
        Task Update();
        Task Delete(Orders order);
    }
}
