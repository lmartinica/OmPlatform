using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Repositories;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IEnumerable<GetProductDto>> GetList(IQueryCollection queryParams)
        {
            IEnumerable<Products>? products;
           
            products = await _cache.GetOrCreateAsync(_cacheName, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repository.GetList();
            });

            // Apply custom filter
            if (queryParams != null && products != null)
            {
                // filter min price
                if (int.TryParse(queryParams["minPrice"], out var min))
                    products = products.Where(p => p.Price >= min);

                // filter max price
                if (int.TryParse(queryParams["maxPrice"], out var max))
                    products = products.Where(p => p.Price <= max);

                // filter stock true, include greater than zero, else 0
                if (bool.TryParse(queryParams["stock"], out var hasStock))
                    products = hasStock ? 
                        products.Where(p => p.Stock > 0) : 
                        products.Where(p => p.Stock == 0);

                // filter category
                if (!string.IsNullOrWhiteSpace(queryParams["category"]))
                    products = products.Where(p => p.Category.Equals(queryParams["category"], StringComparison.OrdinalIgnoreCase));

                // Apply binary search on name
                if (!string.IsNullOrWhiteSpace(queryParams["search"]))
                    products = ServiceBinarySearch(products, queryParams["search"]);
            }

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
            _cache.Remove(_cacheName);
            return Mapper.ToProductDto(createdProduct);
        }

        public async Task<GetProductDto?> Update(Guid id, UpdateProductDto productDto)
        {
            var product = await _repository.GetById(id);
            if (product == null) return null;
            Mapper.UpdateProduct(productDto, product);
            await _repository.Update();
            _cache.Remove(_cacheName);
            return Mapper.ToProductDto(product);
        }

        public async Task<bool> Delete(Guid id)
        {
            var product = await _repository.GetById(id);
            if (product == null) return false;
            await _repository.Delete(product);
            _cache.Remove(_cacheName);
            return true;
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
