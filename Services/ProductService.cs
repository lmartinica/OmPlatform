using OmPlatform.Core;
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
            return products.Select(Mapper.ToProductDto);
        }

        public async Task<GetProductDto?> GetById(Guid id)
        {
            var product = await _repository.GetById(id);
            return product == null ? null : Mapper.ToProductDto(product);
        }

        public async Task<GetProductDto> Create(CreateProductDto productDto)
        {
            var product = Mapper.ToProduct(productDto);
            var createdProduct = await _repository.Create(product);
            return Mapper.ToProductDto(createdProduct);
        }

        public async Task<GetProductDto?> Update(Guid id, UpdateProductDto productDto)
        {
            var product = await _repository.GetById(id);
            if (product == null) return null;
            Mapper.UpdateProduct(productDto, product);
            await _repository.Update();
            return Mapper.ToProductDto(product);
        }

        public async Task<bool> Delete(Guid id)
        {
            var product = await _repository.GetById(id);
            if (product == null) return false;
            await _repository.Delete(product);
            return true;
        }
    }
}
