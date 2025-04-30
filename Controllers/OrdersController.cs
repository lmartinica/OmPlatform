using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Order;
using OmPlatform.Services;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetList()
        {
            var orders = await _orderService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDto>> GetById(Guid id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<GetOrderDto>> Post(CreateOrderDto orderDto)
        {
            var order = await _orderService.Create(orderDto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetOrderDto>> Update(Guid id, UpdateOrderDto orderDto)
        {
            var updatedOrder = await _orderService.Update(id, orderDto);
            if (updatedOrder == null)
                return NotFound();
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            // TODO
            await _orderService.Delete(id);
            //if (!product)
            //    return NotFound();
            return NoContent();
        }
    }
}
