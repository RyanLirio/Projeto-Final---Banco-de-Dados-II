using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IProductOrderRepository
    {
        public Task Create(ProductOrder productOrder);
        public Task Update(int? originalProductId, int? originalOrderId, ProductOrder productOrderNewData);
        public Task Delete(ProductOrder productOrder);
        public Task DeleteByOrder(int orderId);
        public Task<List<ProductOrder?>> GetByProductId(int productId);
        public Task<List<ProductOrder?>> GetByOrderId(int orderId);
        public Task<ProductOrder?> Get(int productId, int orderId);
        public Task<List<ProductOrder>> GetAll();
    }
}
