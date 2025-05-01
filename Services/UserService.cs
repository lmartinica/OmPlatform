using Microsoft.AspNetCore.Identity;
using OmPlatform.Core;
using OmPlatform.DTOs.User;
using OmPlatform.Models;
using OmPlatform.Repositories;

namespace OmPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetUserDto>> GetAll()
        {
            var users = await _repository.GetAll();
            return users.Select(Mapper.ToUserDto);
        }

        public async Task<GetUserDto?> GetByEmailAndPassword(string email, string password)
        {
            var user = await _repository.GetByEmailAndPassword(email, password);
            return user == null ? null : Mapper.ToUserDto(user);
        }

        public async Task<GetUserDto?> GetByEmail(string email)
        {
            var user = await _repository.GetByEmail(email);
            return user == null ? null : Mapper.ToUserDto(user);
        }

        public async Task<GetUserDto?> GetById(Guid id)
        {
            var user = await _repository.GetById(id);
            return user == null ? null : Mapper.ToUserDto(user);
        }

        public async Task<GetUserDto> Create(CreateUserDto userDto)
        {
            var user = Mapper.ToUser(userDto);
            user.Role = "User";

            var hasher = new PasswordHasher<object>();
            user.Password = hasher.HashPassword(user, user.Password);

            var createdUser = await _repository.Create(user);
            return Mapper.ToUserDto(createdUser);
        }

        public async Task<GetUserDto?> Update(Guid id, UpdateUserDto userDto)
        {
            var user = await _repository.GetById(id);
            if (user == null) return null;
            Mapper.UpdateUser(userDto, user);
            await _repository.Update();
            return Mapper.ToUserDto(user);
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return false;
            await _repository.Delete(user);
            return true;
        }
    }
}
