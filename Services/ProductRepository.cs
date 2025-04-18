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

        public async Task<GetProductDto?> Update(UpdateProductDto productDto)
        {
            var product = ToModel(productDto);
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
                // TODO Mapping
            };
        }

        private Products ToModel(CreateProductDto productDto)
        {
            return new Products
            {
                // TODO Mapping
            };
        }

        private Products ToModel(UpdateProductDto productDto)
        {
            return new Products
            {
                // TODO Mapping
            };
        }
    }
}
