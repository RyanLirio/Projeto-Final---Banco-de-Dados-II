using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ICinemaRepository
    {
        public Task Create(Cinema cinema);
        public Task Update(Cinema cinema);
        public Task Delete(Cinema cinema);
        public Task<List<Cinema>> GetByName(string name);
        public Task<Cinema?> GetById(int id);
        public Task<List<Cinema>> GetAll();
    }
}
