using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Queries;
using OmPlatform.Repositories;
using System.Web;
using static OmPlatform.Core.Result<OmPlatform.DTOs.Product.GetProductDto>;

namespace OmPlatform.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly string _cacheName = "products";

        public ProductService(IProductRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<GetProductDto>>> GetList(ProductQuery productQuery)
        {
            IEnumerable<Products>? products;
           
            products = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });

            // Apply custom filter
            if (products != null)
            {
                // filter min price
                if (productQuery.MinPrice != null)
                    products = products.Where(p => p.Price >= productQuery.MinPrice.Value);

                // filter max price
                if (productQuery.MaxPrice != null)
                    products = products.Where(p => p.Price <= productQuery.MaxPrice.Value);

                // filter stock true, include greater than zero, else 0
                if (productQuery.Stock != null)
                    products = productQuery.Stock.Value ? 
                        products.Where(p => p.Stock > 0) : 
                        products.Where(p => p.Stock == 0);

                // filter category
                if (productQuery.Category != null)
                    products = products.Where(p => p.Category.Equals(productQuery.Category, StringComparison.OrdinalIgnoreCase));

                // Apply binary search on name
                if (productQuery.Search != null)
                    products = ServiceBinarySearch(products, productQuery.Search);
            }

            return Result<IEnumerable<GetProductDto>>.Success(products.Select(x => x.ToProductDto()));
        }

        public async Task<Result<GetProductDto>> GetById(Guid id)
        {
            var product = await _repository.GetById(id);
            return product == null ? Failure(404) : Success(product.ToProductDto());
        }

        public async Task<Result<GetProductDto>> Create(CreateProductDto productDto)
        {
            var product = productDto.ToProduct();
            var createdProduct = await _repository.Create(product);
            _cache.Remove(_cacheName);
            return Success(createdProduct.ToProductDto());
        }

        public async Task<Result<GetProductDto>> Update(Guid id, UpdateProductDto productDto)
        {
            var product = await _repository.GetById(id);
            if (product == null) return Failure(404);

            productDto.UpdateProduct(product);
            await _repository.Update();

            _cache.Remove(_cacheName);

            return Success(product.ToProductDto());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            var product = await _repository.GetById(id);
            if (product == null) return Result<bool>.Failure(404);
            await _repository.Delete(product);
            _cache.Remove(_cacheName);
            return Result<bool>.Success(true);
        }

        public IEnumerable<Products> ServiceBinarySearch(IEnumerable<Products> products, string search)
        {
            int left = 0;
            int right = products.Count() - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int compare = string.Compare(products.ElementAt(mid).Name, search, StringComparison.OrdinalIgnoreCase);

                if (compare == 0) return new List<Products> { products.ElementAt(mid) };
                else if (compare < 0) left = mid + 1;
                else right = mid - 1;
            }

            return Enumerable.Empty<Products>();
        }
    }
}
