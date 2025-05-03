using OmPlatform.DTOs.Order;

namespace OmPlatform.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<GetOrderDto>> GetList();
        Task<GetOrderDto?> GetById(Guid id);
        Task<GetOrderDto> Create(CreateOrderDto orderDto);
        Task<GetOrderDto?> Update(Guid id, UpdateOrderDto orderDto);
        Task<bool> Delete(Guid id);
    }
}
