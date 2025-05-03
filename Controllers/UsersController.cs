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
        private readonly IUserContextService _userContextService;

        public UsersController(IUserService userService, IUserContextService userContextService)
        {
            _userService = userService;
            _userContextService = userContextService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetList()
        {
            var users = await _userService.GetList();
            return Ok(users);
        }

        [HttpGet("me")]
        public async Task<ActionResult<GetUserDto>> GetMe()
        {
            var user = await _userService.GetById(_userContextService.GetUserId());
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, [FromBody] UpdateUserDto userDto)
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
