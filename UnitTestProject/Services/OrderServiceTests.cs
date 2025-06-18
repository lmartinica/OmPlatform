using Microsoft.Extensions.Caching.Memory;
using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.OrderItems;
using OmPlatform.Services;
using OmPlatformTest.Fakes;

namespace OmPlatformTest.Services
{
    public class OrderServiceTests
    {
        private readonly Guid _orderIdUser;
        private readonly Guid _orderIdOtherUser;
        private readonly Guid _productId1;
        private readonly Guid _productId2;
        private readonly Guid _productId3;
        private readonly OrderService _service;
        private readonly FakeCurrentUserService _fakeCurrentUser;
        private readonly FakeOrderUnitOfWork _fakeOrderUnitOfWork;
        private readonly MemoryCache _memoryCache;

        public OrderServiceTests() 
        {
            Utilities.SeedData();

            // Add fake orders and products
            var fakeOrderRepo = new FakeOrderRepository();
            var fakeProductRepo = new FakeProductRepository();
            _fakeOrderUnitOfWork = new(fakeOrderRepo, fakeProductRepo);

            fakeOrderRepo.List.AddRange(Utilities.GetSeededOrders());
            fakeProductRepo.List.AddRange(Utilities.GetSeededProducts());

            _fakeCurrentUser = new FakeCurrentUserService(fakeOrderRepo.List[0].UserId, false);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _service = new OrderService(_fakeOrderUnitOfWork, _fakeCurrentUser, _memoryCache);

            _orderIdUser = fakeOrderRepo.List[0].Id;
            _orderIdOtherUser = fakeOrderRepo.List[1].Id;
            _productId1 = fakeProductRepo.List[0].Id;
            _productId2 = fakeProductRepo.List[1].Id;
            _productId3 = fakeProductRepo.List[2].Id;
        }

        public static CreateOrderDto InitOrderDto(params (Guid productId, int quantity)[] items)
        {
            var orderDto = new CreateOrderDto
            {
                OrderItems = []
            };
        
            foreach (var (productId, quantity) in items)
            {
                orderDto.OrderItems.Add(new CreateOrderItemDto
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            return orderDto;
        }

        [Fact]
        public async Task GetList_TestUser_SuccessWithProperData()
        {
            // Arrange
            _fakeCurrentUser.IsAdm = false;

            // Act
            var result = await _service.GetList();

            // Assert
            var orderList = result.Data.ToList();
            Assert.Single(orderList);
            Assert.Equal(_fakeCurrentUser.UserId, orderList[0].UserId);
        }

        [Fact]
        public async Task GetList_TestAdmin_Success()
        {
            // Arrange
            _fakeCurrentUser.IsAdm = true;

            // Act
            var result = await _service.GetList();

            // Assert
            var orderList = result.Data.ToList();
            Assert.True(orderList.Count == 2);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        public async Task GetById_TestUserAndAdmin_Success(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.GetById(_orderIdUser);
            var resultOther = await _service.GetById(_orderIdOtherUser);

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }

        [Fact]
        public async Task Create_TestSuccess()
        {
            // Arrange
            var orderDto = InitOrderDto((_productId3, 1));

            // Act
            var result = await _service.Create(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_TestProductNotFound()
        {
            // Arrange
            var orderDto = InitOrderDto((Guid.NewGuid(), 3));

            // Act
            var result = await _service.Create(orderDto);

            // Assert
            Assert.Equal("NotFound", result.Error.Error.Code);
            // TODO - Evaluate bussiness logic (ex: errors and data)
        }

        [Fact]
        public async Task Create_TestProductLowQuantity()
        {
            // Arrange
            var orderDto = InitOrderDto((_productId1, 2));

            // Act
            var result = await _service.Create(orderDto);

            // Assert
            Assert.Equal("BadRequest", result.Error.Error.Code);
        }

        [Fact]
        public async Task Create_TestNoProduct()
        {
            // Arrange
            var orderDto = InitOrderDto();

            // Act
            var result = await _service.Create(orderDto);

            // Assert
            Assert.Equal("BadRequest", result.Error.Error.Code);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]       
        public async Task Update_TestUserAndAdmin_Success(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.Update(_orderIdUser, new());
            var resultOther = await _service.Update(_orderIdOtherUser, new());

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        public async Task Delete_TestUserAndAdmin_Success(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.Delete(_orderIdUser);
            var resultOther = await _service.Delete(_orderIdOtherUser);

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }
    }
}
