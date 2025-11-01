using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly CineContext _context;

        public LanguageRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Language language)
        {
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Language language)
        {
            _context.Languages.Update(language);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Language language)
        {
            _context.Languages.Remove(language);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Language>> GetAll()
        {
            var data = await _context.Languages.ToListAsync();
            return data;
        }

        public async Task<Language?> GetById(int id)
        {
            var language = await _context.Languages.Where(l => l.Id == id).FirstOrDefaultAsync();
            return language;
        }

        public async Task<List<Language>> GetByName(string name)
        {
            var languages = await _context.Languages.Where(l => l.Name!.ToLower().Contains(name.ToLower())).ToListAsync();

            return languages;
        }
    }
}
