using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class SexRepository : ISexRepository
    {
        private readonly CineContext _context;

        public SexRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Sex sex)
        {
            await _context.Sexes.AddAsync(sex);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Sex sex)
        {
            _context.Sexes.Update(sex);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Sex sex)
        {
            _context.Sexes.Remove(sex);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Sex>> GetAll()
        {
            var data = await _context.Sexes.ToListAsync();
            return data;
        }

        public async Task<Sex?> GetById(int id)
        {
            var sex = await _context.Sexes.Where(l => l.Id == id).FirstOrDefaultAsync();
            return sex;
        }

        public async Task<List<Sex>> GetByName(string name)
        {
            var sexes = await _context.Sexes.Where(l => l.Name!.ToLower().Contains(name.ToLower())).ToListAsync();

            return sexes;
        }
    }
}
