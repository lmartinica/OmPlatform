using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Order;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public AuthController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<GetOrderDto> Login()
        {
            return null;
        }

        [HttpPost]
        public IEnumerable<GetOrderDto> Register()
        {
            return null;
        }
    }
}
