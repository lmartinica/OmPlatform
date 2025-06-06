using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.User;
using OmPlatform.Interfaces;
using OmPlatform.Models;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using static OmPlatform.Core.Result<OmPlatform.DTOs.User.GetUserDto>;

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

        public async Task<Result<IEnumerable<GetUserDto>>> GetList()
        {
            var users = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });
            return Result<IEnumerable<GetUserDto>>.Success(users.Select(x=>x.ToUserDto()));
        }

        public async Task<Result<GetUserDto>> GetByEmailAndPassword(string email, string password)
        {
            var user = await _repository.GetByEmailAndPassword(email, password);
            return user == null ? Failure(404) : Success(user.ToUserDto());
        }

        public async Task<Result<GetUserDto>> GetByEmail(string email)
        {
            var user = await _repository.GetByEmail(email);
            return user == null ? Failure(404) : Success(user.ToUserDto());
        }

        public async Task<Result<GetUserDto>> GetById(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return Failure(404);
            if (!_currentUserService.IsAllowed(user.Id)) 
                return Failure(404);
            return Success(user.ToUserDto());
        }

        public async Task<Result<GetUserDto>> Create(CreateUserDto userDto)
        {
            var user = userDto.ToUser();
            user.Role = Constants.User;

            if (userDto.Password.Length < 5)
                return Failure(400, $"Password too short. It must be at least 5 characters long.");

            var hasher = new PasswordHasher<object>();
            user.Password = hasher.HashPassword(user, user.Password);

            var createdUser = await _repository.Create(user);
            _cache.Remove(_cacheName);
            return Success(createdUser.ToUserDto());
        }

        public async Task<Result<GetUserDto>> Update(Guid id, UpdateUserDto userDto)
        {
            var user = await _repository.GetById(id);
            if (user == null) return Failure(404);
            if (!_currentUserService.IsAllowed(user.Id)) 
                return Failure(404);
            userDto.UpdateUser(user);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return Success(user.ToUserDto());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return Result<bool>.Failure(404);
            if (!_currentUserService.IsAllowed(user.Id)) 
                return Result<bool>.Failure(404);
            await _repository.Delete(user);
            _cache.Remove(_cacheName);
            return Result<bool>.Success(true);
        }
    }
}
