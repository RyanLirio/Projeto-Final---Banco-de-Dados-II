using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class TicketOrderRepository : ITicketOrderRepository
    {
        private readonly CineContext _context;

        public TicketOrderRepository(CineContext movieContext)
        {
            _context = movieContext;
        }

        // --- MÉTODOS DE MANIPULAÇÃO DE DADOS (CUD) ---

        public async Task Create(TicketOrder ticketOrder)
        {
            await _context.TicketOrders.AddAsync(ticketOrder);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? originalTicketId, int? originalOrderId, TicketOrder ticketOrderNewData)
        {
            var ticketOrderOld = await _context.TicketOrders
                .FindAsync(originalTicketId, originalOrderId);

            if (ticketOrderOld != null)
            {
                // Remove o registro antigo
                _context.TicketOrders.Remove(ticketOrderOld);
                await _context.SaveChangesAsync();

                // Adiciona o novo registro com os dados atualizados
                await _context.TicketOrders.AddAsync(ticketOrderNewData);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(TicketOrder ticketOrder)
        {
            _context.TicketOrders.Remove(ticketOrder);
            await _context.SaveChangesAsync();
        }

        // --- MÉTODOS DE CONSULTA (READ) ---

        public async Task<TicketOrder?> Get(int ticketId, int orderId)
        {
            return await _context.TicketOrders
                .Include(x => x.Ticket)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(w => w.TicketId == ticketId && w.TicketId == orderId);
        }

        public async Task<List<TicketOrder>> GetAll()
        {
            return await _context.TicketOrders
                .Include(x => x.Ticket)
                .Include(x => x.Order)
                .ToListAsync();
        }

        public async Task<List<TicketOrder>> GetByOrderId(int orderId)
        {
            return await _context.TicketOrders
                .Include(x => x.Ticket)
                .Include(x => x.Order)
                .Where(w => w.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<TicketOrder>> GetByTicketId(int ticketId)
        {
            return await _context.TicketOrders
                .Include(x => x.Ticket)
                .Include(x => x.Order)
                .Where(w => w.TicketId == ticketId)
                .ToListAsync();
        }
    }
}