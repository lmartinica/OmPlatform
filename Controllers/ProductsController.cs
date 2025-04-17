using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly DbAppContext _context;

        public ProductsController(DbAppContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetList()
        {
            return await _context.Products.ToListAsync();
        }


        [HttpGet("{id}")]
        public ActionResult<GetProductDto> GetById(int id)
        {
            var product = new GetProductDto { Id = id, Name = $"Product {id}", Price = 100};
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult<Products>> Post(Products product)
        {
            _context.Products.Add(product);
            _logger.LogInformation($"Product {product.Id} created: {product.Name}");
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetList), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Products>> Update(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            _logger.LogInformation($"Product {id} updated: {updateProductDto.Name}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"Product {id} deleted");
            return NoContent();
        }
    }
}
