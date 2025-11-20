using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private CineContext _context;

        public TicketRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByOrder(int orderId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.OrderId == orderId)
                .ToListAsync();

            if (tickets.Any())
            {
                _context.Tickets.RemoveRange(tickets);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Ticket>> GetAll()
        {
            var data = await _context.Tickets
                .Include(t => t.Chair)
                    .ThenInclude(c => c!.Room)
                        .ThenInclude(r => r!.Cinema)
                            .ThenInclude(c => c!.Address)
                .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                .Include(t => t.Order)
                    .ThenInclude(o => o!.Client)
                .ToListAsync();
            return data;
        }

        public async Task<Ticket?> GetById(int id)
        {
            var ticket = await _context.Tickets
                .Where(t => t.Id == id)
                .Include(t => t.Chair)
                .Include(t => t.Session)
                .Include(t => t.Order)
                .FirstOrDefaultAsync();
            return ticket;
        }

        public async Task Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetByClientId(int clientId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Chair)
                .Include(t => t.Session)
                .Include(t => t.Order)
                .Where(t => t.Order != null && t.Order.ClientId == clientId)
                .ToListAsync();

            return tickets;
        }

        public async Task<List<Ticket>> GetBySessionId(int sessionId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Chair)
                .Include(t => t.Session)
                .Include(t => t.Order)
                .Where(t => t.Session != null && t.SessionId == sessionId)
                .ToListAsync();

            return tickets;
        }
    }
}
