using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IGenderRepository
    {
        public Task Create(Gender gender);
        public Task Update(Gender gender);
        public Task Delete(Gender gender);
        public Task<List<Gender>> GetByName(string name);
        public Task<Gender?> GetById(int id);
        public Task<List<Gender>> GetAll();
    }
}
