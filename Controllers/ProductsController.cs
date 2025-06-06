using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Interfaces;
using OmPlatform.Models;
using OmPlatform.Queries;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetList(
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] bool? stock,
            [FromQuery] string? category,
            [FromQuery] string? search)
        {
            var productQuery = new ProductQuery()
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Stock = stock,
                Category = category,
                Search = search
            };

            var result = await _productService.GetList(productQuery);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> GetById(Guid id)
        {
            var result = await _productService.GetById(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult<GetProductDto>> Post([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.Create(productDto);
            return Created($"/products/{result.Data.Id}", result.Data);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult<GetProductDto>> Update(Guid id, [FromBody] UpdateProductDto productDto)
        {
            var result = await _productService.Update(id, productDto);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productService.Delete(id);
            if (!result.IsSuccess) return this.ErrorNotFound();
            return NoContent();
        }
    }
}
