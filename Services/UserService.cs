using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.User;
using OmPlatform.Models;
using OmPlatform.Repositories;
using System.Collections.Generic;

namespace OmPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName = "users";

        public UserService(IUserRepository repository, ICurrentUserService currentUserService, IMemoryCache cache)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public async Task<IEnumerable<GetUserDto>> GetList()
        {
            var users = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });
            return users.Select(x=>x.ToUserDto());
        }

        public async Task<GetUserDto?> GetByEmailAndPassword(string email, string password)
        {
            var user = await _repository.GetByEmailAndPassword(email, password);
            return user == null ? null : user.ToUserDto();
        }

        public async Task<GetUserDto?> GetByEmail(string email)
        {
            var user = await _repository.GetByEmail(email);
            return user == null ? null : user.ToUserDto();
        }

        public async Task<GetUserDto?> GetById(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return null;
            if (!_currentUserService.IsAllowed(user.Id)) return null;
            return user.ToUserDto();
        }

        public async Task<GetUserDto> Create(CreateUserDto userDto)
        {
            var user = userDto.ToUser();
            user.Role = Constants.User;

            var hasher = new PasswordHasher<object>();
            user.Password = hasher.HashPassword(user, user.Password);

            var createdUser = await _repository.Create(user);
            _cache.Remove(_cacheName);
            return createdUser.ToUserDto();
        }

        public async Task<GetUserDto?> Update(Guid id, UpdateUserDto userDto)
        {
            var user = await _repository.GetById(id);
            if (user == null) return null;
            if (!_currentUserService.IsAllowed(user.Id)) return null;
            userDto.UpdateUser(user);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return user.ToUserDto();
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return false;
            if (!_currentUserService.IsAllowed(user.Id)) return false;
            await _repository.Delete(user);
            _cache.Remove(_cacheName);
            return true;
        }
    }
}
