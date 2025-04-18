using OmPlatform.DTOs.Order;

namespace OmPlatform.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<GetOrderDto>> GetAll();
        Task<GetOrderDto?> GetById(Guid id);
        Task<GetOrderDto> Create(CreateOrderDto orderDto);
        Task<GetOrderDto?> Update(UpdateOrderDto orderDto);
        Task Delete(Guid id);
    }
}
