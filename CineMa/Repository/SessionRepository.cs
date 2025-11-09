using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private CineContext _context;

        public SessionRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }
         
        public async Task Delete(Session session)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Session>> GetAll()
        {
            var data = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                .Include(s => s.LanguageCaption)
                .ToListAsync();

            return data;
        }

        public async Task<Session?> GetById(int id)
        {
            var session = await _context.Sessions.Where(s => s.Id == id)
                .Include(s => s.Movie)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                .Include(s => s.LanguageCaption)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync();
            return session;
        }

        public async Task Update(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
