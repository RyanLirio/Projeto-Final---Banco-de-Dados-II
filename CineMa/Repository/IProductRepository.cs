using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IProductRepository
    {
        public Task Create(Product product);
        public Task Update(Product product);
        public Task Delete(Product product);
        public Task<Product?> GetById(int id);
        public Task<List<Product>> GetAll();
    }
}
