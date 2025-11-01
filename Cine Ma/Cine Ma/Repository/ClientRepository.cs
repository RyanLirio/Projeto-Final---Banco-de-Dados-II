using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly CineContext _context;

        public ClientRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Client client)
        {
            _context.Clients.Remove(client);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAll()
        {
            var data = await _context.Clients.ToListAsync();
            return data;
        }

        public async Task<Client?> GetById(int id)
        {
            var client = await _context.Clients.Where(p => p.Id == id).FirstOrDefaultAsync();
            return client;
        }

        public async Task<List<Client>> GetByName(string name)
        {
            var clients = await _context.Clients.Where(c => c.Name!.ToLower().Contains(name.ToLower())).ToListAsync();

            return clients;
        }
    }
}
