using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly CineContext _context;

        public CinemaRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Cinema cinema)
        {
            await _context.Cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Cinema cinema)
        {
            _context.Cinemas.Remove(cinema);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Cinema>> GetAll()
        {
            var data = await _context.Cinemas
                .Include(c => c.Address)
                .Include(c=> c.Rooms)
                .ToListAsync();
            return data;
        }

        public async Task<Cinema?> GetById(int id)
        {
            var cinema = await _context.Cinemas
                .Where(c => c.Id == id)
                .Include(c => c.Address)
                .Include(c => c.Rooms)
                .FirstOrDefaultAsync();
            return cinema;
        }

        public async Task<List<Cinema>> GetByName(string name)
        {
            var cinemas = await _context.Cinemas
                .Where(c => c.Name!.ToLower()
                .Contains(name.ToLower()))
                .Include(c => c.Address)
                .Include(c => c.Rooms)
                .ToListAsync();

            return cinemas;
        }
    }
}
