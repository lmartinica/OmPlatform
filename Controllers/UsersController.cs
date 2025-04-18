using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Product;
using OmPlatform.DTOs.User;
using OmPlatform.Services;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetList()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<GetUserDto>> Post(CreateUserDto userDto)
        {
            var user = await _userService.Create(userDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, UpdateUserDto userDto)
        {
            var updatedUser = await _userService.Update(id, userDto);
            if (updatedUser == null)
                return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            // TODO
            await _userService.Delete(id);
            //if (!product)
            //    return NotFound();
            return NoContent();
        }
    }
}
