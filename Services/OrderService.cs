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
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName = "orders";

        public OrderService(IOrderUnitOfWork orderUnitOfWork, ICurrentUserService currentUserService, IMemoryCache cache)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<GetOrderDto>>> GetList()
        {
            var orders = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _orderUnitOfWork.Orders.GetList();
            });

            if (_currentUserService.IsUser()) 
                orders = orders.Where(o => o.UserId == _currentUserService.GetUserId());
            return Result<IEnumerable<GetOrderDto>>.Success(orders.Select(x=>x.ToOrderDto()));
        }

        public async Task<Result<GetOrderDto>> GetById(Guid id)
        {
            var order = await _orderUnitOfWork.Orders.GetById(id);
            if (order == null) return Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId))
                return Failure(404);
            return Success(order.ToOrderDto());
        }

        public async Task<Result<GetOrderDto>> Create(CreateOrderDto orderDto)
        {
            if (orderDto.OrderItems == null || orderDto.OrderItems.Count == 0)
                return Failure(400, "Order must contain at least one product.");

            var order = orderDto.ToOrder();

            order.UserId = _currentUserService.GetUserId();
            order.Created = DateTime.UtcNow;
            order.Status = "Pending";

            // Validate each item stock, reduce the stock.
            foreach (var item in order.OrderItems)
            {
                var product = await _orderUnitOfWork.Products.GetById(item.ProductId);
                if (product == null) 
                    return Failure(404, $"Product with ID {item.ProductId} not found.");
                if (product.Stock < item.Quantity)
                    return Failure(400, $"Not enough stock for product {product.Id}.");
                if (item.Quantity <= 0)
                    return Failure(400,$"Product quantity must be at least 1.");

                product.Stock -= item.Quantity;
                order.TotalPrice += product.Price * item.Quantity;
            }

            var createdOrder = await _orderUnitOfWork.Orders.Create(order);
            await _orderUnitOfWork.CompleteAsync();

            _cache.Remove(_cacheName);
            return Success(createdOrder.ToOrderDto());
        }

        public async Task<Result<GetOrderDto>> Update(Guid id, UpdateOrderDto orderDto)
        {
            var order = await _orderUnitOfWork.Orders.GetById(id);
            if (order == null) return Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId))
                return Failure(404);
            orderDto.UpdateOrder(order);
            await _orderUnitOfWork.Orders.Update();
            _cache.Remove(_cacheName);
            return Success(order.ToOrderDto());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            var order = await _orderUnitOfWork.Orders.GetById(id);
            if (order == null) return Result<bool>.Failure(404);
            if (!_currentUserService.IsAllowed(order.UserId)) 
                return Result<bool>.Failure(404);
            await _orderUnitOfWork.Orders.Delete(order);
            _cache.Remove(_cacheName);
            return Result<bool>.Success(true);
        }
    }
}
