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
            var data = await _context.Orders
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.Movie)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.LanguageAudio)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.LanguageCaption)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Chair)
                        .ThenInclude(c => c!.Room)
                .Include(o => o.Client)
                    .ThenInclude(c => c!.Address)
                .Include(o => o.Cinema)
                    .ThenInclude(c => c!.Address)
                .Include(o => o.ProductOrders)
                    .ThenInclude(po => po.Product)
                .ToListAsync();
            return data;
        }

        public async Task<Order?> GetByIdClient(int clientId, int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.ClientId == clientId)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.Movie)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.LanguageAudio)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Session)
                        .ThenInclude(s => s!.LanguageCaption)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Chair)
                        .ThenInclude(c => c!.Room)
                .Include(o => o.Client)
                    .ThenInclude(c => c!.Address)
                .Include(o => o.Cinema)
                    .ThenInclude(c => c!.Address)
                .Include(o => o.ProductOrders)
                    .ThenInclude(po => po.Product)
                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order?> GetById(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Tickets)
                .Include(o => o.Client)
                .Include(o => o.Cinema)
                .Include(o => o.ProductOrders)
                .FirstOrDefaultAsync();
            return order;
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
