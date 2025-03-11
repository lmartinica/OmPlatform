using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.User;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<GetUserDto> Get()
        {
            return null;
        }
    }
}
