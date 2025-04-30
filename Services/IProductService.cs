using OmPlatform.DTOs.Product;

namespace OmPlatform.Services
{
    public interface IProductService
    {
        Task<IEnumerable<GetProductDto>> GetAll();
        Task<GetProductDto?> GetById(Guid id);
        Task<GetProductDto> Create(CreateProductDto productDto);
        Task<GetProductDto?> Update(Guid id, UpdateProductDto productDto);
        Task<bool> Delete(Guid id);
    }
}
