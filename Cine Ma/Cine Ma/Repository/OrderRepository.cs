using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private CineContext _context;

        public OrderRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAll()
        {
            var data = await _context.Orders.ToListAsync(); 
            return data;
        }

        public async Task<Order?> GetById(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            return order;
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
