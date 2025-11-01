using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class GenderRepository : IGenderRepository
    {
        private readonly CineContext _context;

        public GenderRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Gender gender)
        {
            await _context.Genders.AddAsync(gender);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Gender gender)
        {
            _context.Genders.Update(gender);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Gender gender)
        {
            _context.Genders.Remove(gender);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Gender>> GetAll()
        {
            var data = await _context.Genders.ToListAsync();
            return data;
        }

        public async Task<Gender?> GetById(int id)
        {
            var gender = await _context.Genders.Where(l => l.Id == id).FirstOrDefaultAsync();
            return gender;
        }

        public async Task<List<Gender>> GetByName(string name)
        {
            var genders = await _context.Genders.Where(l => l.Name!.ToLower().Contains(name.ToLower())).ToListAsync();

            return genders;
        }
    }
}
