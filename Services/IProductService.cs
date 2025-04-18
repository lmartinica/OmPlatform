using OmPlatform.DTOs.Product;

namespace OmPlatform.Services
{
    public interface IProductService
    {
        Task<IEnumerable<GetProductDto>> GetAll();
        Task<GetProductDto?> GetById(Guid id);
        Task<GetProductDto> Create(CreateProductDto productDto);
        Task<GetProductDto?> Update(UpdateProductDto productDto);
        Task Delete(Guid id);
    }
}
