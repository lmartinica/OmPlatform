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
            return orders.Select(ToGetDto);
        }

        public async Task<GetOrderDto?> GetById(Guid id)
        {
            var order = await _repository.GetById(id);
            return order == null ? null : ToGetDto(order);
        }

        public async Task<GetOrderDto> Create(CreateOrderDto orderDto)
        {
            var order = ToModel(orderDto);
            var createdOrder = await _repository.Create(order);
            return ToGetDto(createdOrder);

        }

        public async Task<GetOrderDto?> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = ToModel(orderDto, id);
            var updatedOrder = await _repository.Update(order);
            return updatedOrder == null ? null : ToGetDto(updatedOrder);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }


        private GetOrderDto ToGetDto(Orders order)
        {
            return new GetOrderDto
            {
                // TODO Mapping
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Created = order.Created,
                Items = null
            };
        }

        private Orders ToModel(CreateOrderDto orderDto)
        {
            return new Orders
            {
                // TODO Mapping
            };
        }

        private Orders ToModel(UpdateOrderDto orderDto, Guid id)
        {
            return new Orders
            {
                // TODO Mapping
            };
        }
    }
}
