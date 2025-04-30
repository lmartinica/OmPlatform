using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly DbAppContext _context;

        public ProductRepository(DbAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Products>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products?> GetById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Products> Create(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Products?> Update(Products product)
        {
            var productFound = await _context.Products.FindAsync(product.Id);
            if (productFound != null)
            {
                // TODO: state nu trebuie modificat, trimite eroare chat
                _context.Entry(productFound).State = EntityState.Detached;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return product;
            }
            return null;
        }

        public async Task Delete(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
