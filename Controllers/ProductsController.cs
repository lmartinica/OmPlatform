using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
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
        public async Task<ActionResult<Products>> GetById(Guid id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }


        [HttpPost]
        public async Task<ActionResult<Products>> Post(Products product)
        {
            _context.Products.Add(product);
            _logger.LogInformation($"Product {product.Id} created: {product.Name}");
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetList), new { id = product.Id }, product);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Products>> Update(Guid id, [FromBody] Products product)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Price = product.Price;
            entity.Stock = product.Stock;
            entity.Category = product.Category;

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation($"Product {id} updated: {product.Name}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product {id} deleted");
            return NoContent();
        }
    }
}
