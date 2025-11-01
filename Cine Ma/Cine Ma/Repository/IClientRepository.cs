using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IClientRepository
    {
        public Task Create(Client client);
        public Task Update(Client client);
        public Task Delete(Client client);
        public Task<List<Client>> GetByName(string name);
        public Task<Client?> GetById(int id);
        public Task<List<Client>> GetAll();
    }
}
