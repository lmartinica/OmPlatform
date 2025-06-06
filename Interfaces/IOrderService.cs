using OmPlatform.Core;
using OmPlatform.DTOs.Order;

namespace OmPlatform.Interfaces
{
    public interface IOrderService
    {
        Task<Result<IEnumerable<GetOrderDto>>> GetList();
        Task<Result<GetOrderDto>> GetById(Guid id);
        Task<Result<GetOrderDto>> Create(CreateOrderDto orderDto);
        Task<Result<GetOrderDto>> Update(Guid id, UpdateOrderDto orderDto);
        Task<Result<bool>> Delete(Guid id);
    }
}
