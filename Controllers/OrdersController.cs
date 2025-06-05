using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.Services;
using System.Security.Claims;

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
            var orders = await _orderService.GetList();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDto>> GetById(Guid id)
        {
            var order = await _orderService.GetById(id);
            if (order == null) return this.ErrorNotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<GetOrderDto>> Post([FromBody] CreateOrderDto orderDto)
        {
            var result = await _orderService.Create(orderDto);
            if (!result.IsSuccess) return this.Error(result);
            return Created($"/orders/{result.Data.Id}", result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetOrderDto>> Update(Guid id, [FromBody] UpdateOrderDto orderDto)
        {
            var order = await _orderService.Update(id, orderDto);
            if (order == null) return this.ErrorNotFound();
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _orderService.Delete(id);
            if (!result) return this.ErrorNotFound();
            return NoContent();
        }
    }
}
