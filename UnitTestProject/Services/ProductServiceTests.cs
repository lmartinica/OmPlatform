using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Queries;
using OmPlatform.Services;
using OmPlatformTest.Fakes;

namespace OmPlatformTest.Services
{
    public class ProductServiceTests
    {
        private readonly Guid _productId;
        private readonly ProductService _service;
        private readonly FakeProductRepository _fakeProductRepo;
        private readonly MemoryCache _memoryCache;

        public ProductServiceTests()
        {
            Utilities.SeedData();

            // Add fake products
            _fakeProductRepo = new FakeProductRepository();
            _fakeProductRepo.List.AddRange(Utilities.GetSeededProducts());

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _service = new ProductService(_fakeProductRepo, _memoryCache);
            _productId = _fakeProductRepo.List[0].Id;
        }

        [Fact]
        public async Task GetList_TestSuccessWithQuery()
        {
            // Arrange
            var query = new ProductQuery()
            {
                MinPrice = 10,
                MaxPrice = 20,
                Stock = false,
                Category = "1",
            };

            var search = new ProductQuery() { Search = "Test1" };

            // Act
            var resultQuery = await _service.GetList(query);
            var resultSearch = await _service.GetList(search);

            // Assert
            Assert.Single(resultQuery.Data);
            Assert.Single(resultSearch.Data);
        }

        [Fact]
        public async Task GetbyId_TestSuccess()
        {
            // Act
            var result = await _service.GetById(_productId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_TestSuccess()
        {
            // Act
            var result = await _service.Create(new());

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Update_TestSuccess()
        {
            // Act
            var result = await _service.Update(_productId, new());

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_TestSuccess()
        {
            // Act
            var result = await _service.Delete(_productId);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
