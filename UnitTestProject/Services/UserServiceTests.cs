using Microsoft.Extensions.Caching.Memory;
using OmPlatform.DTOs.User;
using OmPlatform.Services;
using OmPlatformTest.Fakes;

namespace OmPlatformTest.Services
{
    public class UserServiceTests
    {
        private readonly Guid _userId;
        private readonly Guid _userIdOther;
        private readonly UserService _service;
        private readonly FakeUserRepository _fakeUserRepo;
        private readonly FakeCurrentUserService _fakeCurrentUser;
        private readonly MemoryCache _memoryCache;

        public UserServiceTests()
        {
            Utilities.SeedData();

            // Add fake users
            _fakeUserRepo = new FakeUserRepository();
            _fakeUserRepo.List.AddRange(Utilities.GetSeededUsers());

            _userId = _fakeUserRepo.List[0].Id;
            _userIdOther = _fakeUserRepo.List[1].Id;

            _fakeCurrentUser = new FakeCurrentUserService(_userId, false);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _service = new UserService(_fakeUserRepo, _fakeCurrentUser, _memoryCache);
        }

        [Fact]
        public async Task GetList_TestSuccess()
        {
            // Act
            var result = await _service.GetList();

            // Assert
            var orderList = result.Data.ToList();
            Assert.True(orderList.Count == 2);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        public async Task GetById_TestAdminAndUserSuccess(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.GetById(_userId);
            var resultOther = await _service.GetById(_userIdOther);

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }

        [Fact]
        public async Task GetByEmail_TestSuccess()
        {
            // Act
            var result = await _service.GetByEmail("user1@mail");

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetByEmailAndPassword_TestSuccess()
        {
            // Act
            var result = await _service.GetByEmailAndPassword("user1@mail", "12345");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_TestSuccess()
        {
            // Arrange
            var userDto = new CreateUserDto { Password = "12345" };

            // Act
            var result = await _service.Create(userDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_TestPasswordPolicyNoAccess()
        {
            // Arrange
            var userDto = new CreateUserDto { Password = "123" };

            // Act
            var result = await _service.Create(userDto);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        public async Task Update_TestAdminAndUserSuccess(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.Update(_userId, new());
            var resultOther = await _service.Update(_userIdOther, new());

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        public async Task Delete_TestAdminAndUserSuccess(bool isAdmin, bool expectUserResult, bool expectOtherResult)
        {
            // Arrange
            _fakeCurrentUser.IsAdm = isAdmin;

            // Act
            var resultUser = await _service.Delete(_userId);
            var resultOther = await _service.Delete(_userIdOther);

            // Assert
            Assert.Equal(expectUserResult, resultUser.IsSuccess);
            Assert.Equal(expectOtherResult, resultOther.IsSuccess);
        }
    }
}
