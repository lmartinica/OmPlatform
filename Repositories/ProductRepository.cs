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

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Products product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
