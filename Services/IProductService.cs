using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Queries;

namespace OmPlatform.Services
{
    public interface IProductService
    {
        Task<Result<IEnumerable<GetProductDto>>> GetList(ProductQuery productQuery);
        Task<Result<GetProductDto>> GetById(Guid id);
        Task<Result<GetProductDto>> Create(CreateProductDto productDto);
        Task<Result<GetProductDto>> Update(Guid id, UpdateProductDto productDto);
        Task<Result<bool>> Delete(Guid id);
    }
}
