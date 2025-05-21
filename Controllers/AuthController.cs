using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OmPlatform.DTOs.Auth;
using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.User;
using OmPlatform.Models;
using OmPlatform.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            var user = await _userService.GetByEmailAndPassword(userLoginDto.Email, userLoginDto.Password);

            if (user != null)
            {
                var token = _authService.GenerateJwtToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var user = await _userService.GetByEmail(createUserDto.Email);
            if (user != null) return Unauthorized();
            // TODO password valid, min 5 char, min 1 char special, min 1 numar
            var newUser = await _userService.Create(createUserDto);
            return Created($"/users/{newUser.Id}", newUser);
        }
    }
}
