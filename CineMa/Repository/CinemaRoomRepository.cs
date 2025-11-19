using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class CinemaRoomRepository : ICinemaRoomRepository
    {
        private readonly CineContext _context;

        public CinemaRoomRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(CinemaRoom cinemaRoom)
        {
            await _context.CinemaRooms.AddAsync(cinemaRoom);
            await _context.SaveChangesAsync();
        }

        public async Task Update(CinemaRoom cinemaRoom)
        {
            _context.CinemaRooms.Update(cinemaRoom);
            await _context.SaveChangesAsync();
        }

        public Task Delete(CinemaRoom cinemaRoom)
        {
            _context.CinemaRooms.Remove(cinemaRoom);
            return _context.SaveChangesAsync();
        }

        public async Task<List<CinemaRoom>> GetAll()
        {
            var data = await _context.CinemaRooms
                .Include(r => r.Chairs)
                .Include(r => r.Cinema)
                    .ThenInclude(c => c!.Address)
                .ToListAsync();
            return data;
        }

        public async Task<CinemaRoom?> GetById(int id)
        {
            var cinemaRoom = await _context.CinemaRooms
                .Where(c => c.Id == id)
                .Include(r => r.Chairs)
                .Include(r => r.Cinema)
                    .ThenInclude(c => c!.Address)
                .FirstOrDefaultAsync();
            return cinemaRoom;
        }

        public async Task<List<CinemaRoom>> GetByCinemaId(int id)
        {
            return await _context.CinemaRooms
                .Where(c => c.CinemaId == id)
                .Include(r => r.Chairs)
                .ToListAsync();
        }
    }
}