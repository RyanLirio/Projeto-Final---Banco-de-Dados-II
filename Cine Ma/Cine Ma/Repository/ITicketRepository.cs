using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ITicketRepository
    {
        public Task Create(Ticket ticket);
        public Task Update(Ticket ticket);
        public Task Delete(Ticket ticket);
        public Task<Ticket?> GetById(int id);
        public Task<List<Ticket>> GetAll();
    }
}
