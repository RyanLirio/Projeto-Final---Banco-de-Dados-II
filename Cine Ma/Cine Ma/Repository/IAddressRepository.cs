using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IAddressRepository
    {
        public Task Create(Address address);
        public Task Update(Address address);
        public Task Delete(Address address);
        public Task<List<Address>> GetByCityName(string name);
        public Task<Address?> GetById(int id);
        public Task<List<Address>> GetAll();
    }
}
