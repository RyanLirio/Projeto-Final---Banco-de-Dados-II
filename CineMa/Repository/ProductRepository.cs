using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class ProductRepository : IProductRepository
    {
        private CineContext _context;

        public ProductRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> GetAll()
        {
            var data = await _context.Products.ToListAsync();
            return data;
        }


        public async Task<Product?> GetById(int id)
        {
            var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
