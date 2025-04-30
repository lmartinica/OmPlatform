using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Repositories;

namespace OmPlatform.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetProductDto>> GetAll()
        {
            var products = await _repository.GetAll();
            return products.Select(ToGetDto);
        }

        public async Task<GetProductDto?> GetById(Guid id)
        {
            var product = await _repository.GetById(id);
            return product == null ? null : ToGetDto(product);
        }

        public async Task<GetProductDto> Create(CreateProductDto productDto)
        {
            var product = ToModel(productDto);
            var createdProduct = await _repository.Create(product);
            return ToGetDto(createdProduct);

        }

        public async Task<GetProductDto?> Update(Guid id, UpdateProductDto productDto)
        {
            // TODO: Review model conversion update here
            // Take entity from database model not bussiness logic
            // Validare in bussiness layer
            var product = ToModel(productDto, id);
            var updatedProduct = await _repository.Update(product);
            return updatedProduct == null ? null : ToGetDto(updatedProduct);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }


        private GetProductDto ToGetDto(Products product)
        {
            return new GetProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category
            };
        }

        private Products ToModel(CreateProductDto productDto)
        {
            return new Products
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Category = productDto.Category
            };
        }

        private Products ToModel(UpdateProductDto productDto, Guid id)
        {
            return new Products
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Category = productDto.Category
            };
        }
    }
}
