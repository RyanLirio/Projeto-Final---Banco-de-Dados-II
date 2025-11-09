using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cine_Ma.Repository
{
    public class ChairRepository : IChairRepository
    {
        private readonly CineContext _context;

        public ChairRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Chair chair)
        {
            await _context.Chairs.AddAsync(chair);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Chair chair)
        {
            _context.Chairs.Update(chair);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Chair chair)
        {
            _context.Chairs.Remove(chair);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Chair>> GetAll()
        {
            var data = await _context.Chairs
                .Include(c => c.Room)
                .ToListAsync();
            return data;
        }

        public async Task<List<Chair>?> GetByColumn(int column, int roomId)
        {
            var chairs = await _context.Chairs
                .Where(c => c.RoomId == roomId && c.Column == column)
                .Include(c => c.Room)
                .AsNoTracking()
                .OrderBy(c => c.Row)
                .ToListAsync();

            return chairs.Count == 0 ? null : chairs;
        }

        public async Task<List<Chair>?> GetByRow(string row, int roomId)
        {
            var chairs = await _context.Chairs
                .Where(
                    c => c.Row!.ToLower()
                    .Contains(row.ToLower()) 
                    && c.RoomId == roomId
                )
                .Include(c => c.Room)
                .ToListAsync();

            return chairs;
        }

        public async Task<List<Chair>?> GetByRoom(int roomId)
        {
            var chairs = await _context.Chairs
                .Where(c => c.RoomId == roomId)
                .OrderBy(c => c.Row)
                .Include(c => c.Room)
                .ToListAsync();

            return chairs.Count == 0 ? null : chairs;
        }
    }
}
