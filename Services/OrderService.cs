using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.Models;
using OmPlatform.Repositories;

namespace OmPlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetOrderDto>> GetAll()
        {
            var orders = await _repository.GetAll();
            return orders.Select(Mapper.ToOrderDto);
        }

        public async Task<GetOrderDto?> GetById(Guid id)
        {
            var order = await _repository.GetById(id);
            return order == null ? null : Mapper.ToOrderDto(order);
        }

        public async Task<GetOrderDto> Create(CreateOrderDto orderDto, Guid userId)
        {
            var order = Mapper.ToOrder(orderDto);

            order.UserId = userId;
            order.Created = DateTime.UtcNow;
            order.Status = 0;

            // TODO:
            // Validate stock availability
            // Reduce the stock accordingly.
            // Total price, and order status (Pending, Shipped, Delivered, Canceled).

            var createdOrder = await _repository.Create(order);
            return Mapper.ToOrderDto(createdOrder);
        }

        public async Task<GetOrderDto?> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = await _repository.GetById(id);
            if (order == null) return null;
            Mapper.UpdateOrder(orderDto, order);
            await _repository.Update();
            return Mapper.ToOrderDto(order);
        }

        public async Task<bool> Delete(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return false;
            await _repository.Delete(order);
            return true;
        }
    }
}
