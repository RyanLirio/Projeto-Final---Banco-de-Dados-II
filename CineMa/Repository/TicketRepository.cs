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

        public async Task<List<Ticket>> GetAll()
        {
            var data = await _context.Tickets
                .Include(t => t.Chair)
                .Include(t => t.Session)
                .Include(t => t.Order)
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
    }
}
