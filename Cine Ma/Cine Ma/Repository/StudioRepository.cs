using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class StudioRepository : IStudioRepository
    {
        
        private CineContext _context;

        public StudioRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Studio studio)
        {
            await _context.Studios.AddAsync(studio);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Studio studio)
        {
            _context.Studios.Remove(studio);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Studio>> GetAll()
        {
            var data = await _context.Studios.ToListAsync();
            return data;
        }

        public async Task<Studio?> GetById(int id)
        {
            var studio = await _context.Studios.Where(s=>s.Id == id).FirstOrDefaultAsync();
            return studio;
        }

        public async Task Update(Studio studio)
        {
            _context.Studios.Update(studio);
            await _context.SaveChangesAsync();
        }
    }
}
