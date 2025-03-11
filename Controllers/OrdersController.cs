using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Order;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<GetOrderDto> Get()
        {
            return null;
        }
    }
}
