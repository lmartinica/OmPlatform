using Microsoft.AspNetCore.Mvc;
using OmPlatform.Core;
using OmPlatform.DTOs.Auth;
using OmPlatform.DTOs.User;
using OmPlatform.Interfaces;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _userService.GetByEmailAndPassword(userLoginDto.Email, userLoginDto.Password);

            if (result.IsSuccess)
            {
                var token = _authService.GenerateJwtToken(result.Data);
                return Ok(new { token });
            }
            return this.ErrorUnauthorized("Incorrect credentials");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var result = await _userService.GetByEmail(createUserDto.Email);
            if (result.IsSuccess) return this.ErrorUnauthorized("Email address already used");

            var resultCreate = await _userService.Create(createUserDto);
            if (!resultCreate.IsSuccess) return this.Error(resultCreate);

            return Created($"{Constants.RouteUser}/{resultCreate.Data.Id}", resultCreate.Data);
        }
    }
}
