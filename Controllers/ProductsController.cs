using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Product;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<GetProductDto> GetList()
        {
            // Test Data
            return new List<GetProductDto>
            {
                new GetProductDto { Id = 1, Name = "Product 1", Price = 100, Stock = 1 },
                new GetProductDto { Id = 2, Name = "Product 2", Price = 200, Stock = 1 }
            };
        }

        [HttpGet("{id}")]
        public ActionResult<GetProductDto> GetById(int id)
        {
            var product = new GetProductDto { Id = id, Name = $"Product {id}", Price = 100};
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateProductDto createProductDto)
        {
            _logger.LogInformation($"Product created: {createProductDto.Name}");
            return CreatedAtAction(nameof(GetById), new { id = 1 }, createProductDto);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateProductDto updateProductDto)
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
