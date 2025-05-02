using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.Models;
using OmPlatform.Repositories;

namespace OmPlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName = "orders";

        public OrderService(IOrderRepository repository, IProductRepository productRepository, IMemoryCache cache)
        {
            _repository = repository;
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<GetOrderDto>> GetAll()
        {
            var orders = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetAll();
            });
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
            order.Status = "Pending";

            // Validate each item stock, reduce the stock.
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetById(item.ProductId);
                if (product == null) 
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                if (product.Stock < item.Quantity) 
                    throw new Exception($"Not enough stock for product {product.Name}.");

                product.Stock -= item.Quantity;
                await _productRepository.Update();
                order.TotalPrice += product.Price * item.Quantity;
            }

            var createdOrder = await _repository.Create(order);
            _cache.Remove(_cacheName);
            return Mapper.ToOrderDto(createdOrder);
        }

        public async Task<GetOrderDto?> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = await _repository.GetById(id);
            if (order == null) return null;
            Mapper.UpdateOrder(orderDto, order);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return Mapper.ToOrderDto(order);
        }

        public async Task<bool> Delete(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return false;
            await _repository.Delete(order);
            _cache.Remove(_cacheName);
            return true;
        }
    }
}
