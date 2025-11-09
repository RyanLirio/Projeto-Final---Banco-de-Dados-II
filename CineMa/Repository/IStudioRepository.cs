using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IStudioRepository
    {
        public Task Create(Studio studio);
        public Task Update(Studio studio);
        public Task Delete(Studio studio);
        public Task<Studio?> GetById(int id);
        public Task<List<Studio>> GetAll();
    }
}
