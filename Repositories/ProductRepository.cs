using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.Interfaces;
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

        public async Task<IEnumerable<Products>> GetList()
        {
            return await _context.Products.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<Products?> GetById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Products> Create(Products product)
        {
            _context.Products.Add(product);
            return product;
        }

        public async Task Delete(Products product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
