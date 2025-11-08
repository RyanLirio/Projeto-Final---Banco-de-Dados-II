using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ILanguageRepository
    {
        public Task Create(Language language);
        public Task Update(Language language);
        public Task Delete(Language language);
        public Task<List<Language>> GetByName(string name);
        public Task<Language?> GetById(int id);
        public Task<List<Language>> GetAll();
    }
}
