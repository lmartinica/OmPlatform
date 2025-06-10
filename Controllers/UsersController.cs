using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.Core;
using OmPlatform.DTOs.User;
using OmPlatform.Interfaces;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetList()
        {
            var result = await _userService.GetList();
            return Ok(result.Data);
        }

        [HttpGet("me")]
        public async Task<ActionResult<GetUserDto>> GetMe()
        {
            var result = await _userService.GetById(_currentUserService.GetUserId());
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, [FromBody] UpdateUserDto userDto)
        {
            var result = await _userService.Update(id, userDto);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return NoContent();
        }
    }
}
