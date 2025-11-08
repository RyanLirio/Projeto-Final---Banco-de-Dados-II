using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ITicketOrderRepository
    {
        public Task Create(TicketOrder ticketOrder);
        public Task Update(int? originalTicketId, int? originalOrderId, TicketOrder ticketOrderNewData);
        public Task Delete(TicketOrder ticketOrder);
        public Task<List<TicketOrder?>> GetByTicketId(int ticketId);
        public Task<List<TicketOrder?>> GetByOrderId(int orderId);
        public Task<TicketOrder?> Get(int ticketId, int orderId);
        public Task<List<TicketOrder>> GetAll();
    }
}