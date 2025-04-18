using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Services;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetList()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> GetById(Guid id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult<GetProductDto>> Post(CreateProductDto productDto)
        {
            var product = await _productService.Create(productDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GetProductDto>> Update(Guid id, UpdateProductDto productDto)
        {
            var updatedProduct = await _productService.Update(id, productDto);
            if (updatedProduct == null)
                return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            // TODO
            await _productService.Delete(id);
            //if (!product)
            //    return NotFound();
            return NoContent();
        }
    }
}
