using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ISexRepository
    {
        public Task Create(Sex gender);
        public Task Update(Sex gender);
        public Task Delete(Sex gender);
        public Task<List<Sex>> GetByName(string name);
        public Task<Sex?> GetById(int id);
        public Task<List<Sex>> GetAll();
    }
}
