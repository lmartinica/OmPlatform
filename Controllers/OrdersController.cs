using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.Interfaces;

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
            var result = await _orderService.GetList();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDto>> GetById(Guid id)
        {
            var result = await _orderService.GetById(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<GetOrderDto>> Post([FromBody] CreateOrderDto orderDto)
        {
            var result = await _orderService.Create(orderDto);
            if (!result.IsSuccess) return this.Error(result);
            return Created($"/orders/{result.Data.Id}", result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetOrderDto>> Update(Guid id, [FromBody] UpdateOrderDto orderDto)
        {
            var result = await _orderService.Update(id, orderDto);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _orderService.Delete(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return NoContent();
        }
    }
}
