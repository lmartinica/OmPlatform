using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Services;

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
            // TODO add here ProductQuery productQuery = new() with constructor
            var products = await _productService.GetList(HttpContext.Request.Query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> GetById(Guid id)
        {
            var product = await _productService.GetById(id);
            if (product == null) return this.ErrorNotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult<GetProductDto>> Post([FromBody] CreateProductDto productDto)
        {
            var product = await _productService.Create(productDto);
            return Created($"/products/{product.Id}", product);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult<GetProductDto>> Update(Guid id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _productService.Update(id, productDto);
            if (product == null) return this.ErrorNotFound();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Admin)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productService.Delete(id);
            if (!result) return this.ErrorNotFound();
            return NoContent();
        }
    }
}
