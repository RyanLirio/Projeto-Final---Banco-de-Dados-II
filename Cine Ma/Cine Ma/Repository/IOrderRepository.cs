using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IOrderRepository
    {
        public Task Create(Order order);
        public Task Update(Order order);
        public Task Delete(Order order);
        public Task<Order?> GetById(int id);
        public Task<List<Order>> GetAll();
    }
}
