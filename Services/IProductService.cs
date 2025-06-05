using OmPlatform.DTOs.Product;
using OmPlatform.Queries;

namespace OmPlatform.Services
{
    public interface IProductService
    {
        Task<IEnumerable<GetProductDto>> GetList(ProductQuery productQuery);
        Task<GetProductDto?> GetById(Guid id);
        Task<GetProductDto> Create(CreateProductDto productDto);
        Task<GetProductDto?> Update(Guid id, UpdateProductDto productDto);
        Task<bool> Delete(Guid id);
    }
}
