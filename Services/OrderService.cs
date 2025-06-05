using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.Models;
using OmPlatform.Repositories;
using static OmPlatform.Core.Result<OmPlatform.DTOs.Order.GetOrderDto>;

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

        public async Task<Result<IEnumerable<GetOrderDto>>> GetList()
        {
            var orders = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });

            if (_currentUserService.IsUser()) 
                orders = orders.Where(o => o.UserId == _currentUserService.GetUserId());
            return Result<IEnumerable<GetOrderDto>>.Success(orders.Select(x=>x.ToOrderDto()));
        }

        public async Task<Result<GetOrderDto>> GetById(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId))
                return Failure(404);
            return Success(order.ToOrderDto());
        }

        public async Task<Result<GetOrderDto>> Create(CreateOrderDto orderDto)
        {
            var order = orderDto.ToOrder();

            order.UserId = _currentUserService.GetUserId();
            order.Created = DateTime.UtcNow;
            order.Status = "Pending";

            // Validate each item stock, reduce the stock.
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetById(item.ProductId);
                if (product == null) 
                    return Failure(404, $"Product with ID {item.ProductId} not found.");
                if (product.Stock < item.Quantity)
                    return Failure(400, $"Not enough stock for product {product.Id}.");
                if (item.Quantity <= 0)
                    return Failure(400,$"Quantity must be higher than 0 for product {product.Id}.");

                // TODO update not in loop check (transactions) link unitOfWork
                product.Stock -= item.Quantity;
                await _productRepository.Update();
                order.TotalPrice += product.Price * item.Quantity;
            }

            var createdOrder = await _repository.Create(order);
            // TODO unitOfWork above
            _cache.Remove(_cacheName);
            return Success(createdOrder.ToOrderDto());
        }

        public async Task<Result<GetOrderDto>> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = await _repository.GetById(id);
            if (order == null) return Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId))
                return Failure(404);
            orderDto.UpdateOrder(order);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return Success(order.ToOrderDto());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return Result<bool>.Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId)) 
                return Result<bool>.Failure(404);
            await _repository.Delete(order);
            _cache.Remove(_cacheName);
            return Result<bool>.Success(true);
        }
    }
}
