using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Product;
using OmPlatform.DTOs.User;
using OmPlatform.Models;
using OmPlatform.Services;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetList()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, UpdateUserDto userDto)
        {
            var user = await _userService.Update(id, userDto);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
