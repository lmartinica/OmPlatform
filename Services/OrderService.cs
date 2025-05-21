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
        private readonly ICurrentUserService _currentUserService;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName = "orders";

        public OrderService(IOrderRepository repository, IProductRepository productRepository, ICurrentUserService currentUserService, IMemoryCache cache)
        {
            _repository = repository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public async Task<IEnumerable<GetOrderDto>> GetList()
        {
            var orders = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });

            if (_currentUserService.IsUser()) 
                orders = orders.Where(o => o.UserId == _currentUserService.GetUserId());
            return orders.Select(x=>x.ToOrderDto());
        }

        public async Task<GetOrderDto?> GetById(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return null;
            if (!_currentUserService.IsAllowed(order.UserId)) return null;
            return order.ToOrderDto();
        }

        public async Task<GetOrderDto> Create(CreateOrderDto orderDto)
        {
            var order = orderDto.ToOrder();

            order.UserId = _currentUserService.GetUserId();
            order.Created = DateTime.UtcNow;
            order.Status = "Pending";

            // Validate each item stock, reduce the stock.
            foreach (var item in order.OrderItems)
            {
                // TODO return erroDto, do not return exceptions in service or repository

                var product = await _productRepository.GetById(item.ProductId);
                if (product == null) 
                    throw new HttpException(400, $"Product with ID {item.ProductId} not found.");
                if (product.Stock < item.Quantity) 
                    throw new HttpException(400, $"Not enough stock for product {product.Name}.");
                if (item.Quantity <= 0)
                    throw new HttpException(400, $"Quantity must be higher than 0 for product {product.Name}.");

                // TODO update nu in loop check (transactions) link unitOfWork
                product.Stock -= item.Quantity;
                await _productRepository.Update();
                order.TotalPrice += product.Price * item.Quantity;
            }

            var createdOrder = await _repository.Create(order);
            // TODO unitOfWork above
            _cache.Remove(_cacheName);
            return createdOrder.ToOrderDto();
        }

        public async Task<GetOrderDto?> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = await _repository.GetById(id);
            if (order == null) return null;
            if (!_currentUserService.IsAllowed(order.UserId)) return null;
            orderDto.UpdateOrder(order);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return order.ToOrderDto();
        }

        public async Task<bool> Delete(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return false;
            if (!_currentUserService.IsAllowed(order.UserId)) return false;
            await _repository.Delete(order);
            _cache.Remove(_cacheName);
            return true;
        }
    }
}
