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
            return users.Select(ToGetDto);
        }

        public async Task<GetUserDto?> GetById(Guid id)
        {
            var user = await _repository.GetById(id);
            return user == null ? null : ToGetDto(user);
        }

        public async Task<GetUserDto> Create(CreateUserDto userDto)
        {
            var user = ToModel(userDto);
            var createdUser = await _repository.Create(user);
            return ToGetDto(createdUser);
        }

        public async Task<GetUserDto?> Update(Guid id, UpdateUserDto userDto)
        {
            var user = ToModel(userDto, id);
            var updatedUser = await _repository.Update(user);
            return updatedUser == null ? null : ToGetDto(updatedUser);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }


        private GetUserDto ToGetDto(Users user)
        {
            return new GetUserDto
            {
                // TODO Mapping
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            };
        }

        private Users ToModel(CreateUserDto userDto)
        {
            return new Users
            {
                // TODO Mapping
            };
        }

        private Users ToModel(UpdateUserDto userDto, Guid id)
        {
            return new Users
            {
                // TODO Mapping
            };
        }
    }
}
