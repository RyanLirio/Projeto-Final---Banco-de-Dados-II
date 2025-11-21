using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    .ThenInclude(m => m!.Language)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                    .ThenInclude(r => r!.Cinema)
                        .ThenInclude(c => c!.Address)
                .Include(s => s.LanguageCaption)
                .ToListAsync();

            return data;
        }

        public async Task<Session?> GetById(int id)
        {
            var session = await _context.Sessions.Where(s => s.Id == id)
                .Include(s => s.Movie)
                    .ThenInclude(m => m!.Language)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                    .ThenInclude(r => r!.Cinema)
                        .ThenInclude(c => c!.Address)
                .Include(s => s.LanguageCaption)
                .FirstOrDefaultAsync();
            return session;
        }

        public async Task<List<Session>?> GetActiveSessions()
        {
            var data = await _context.Sessions
                .Where(s => s.SessionHour > DateTime.Now)
                .Include(s => s.Movie)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                .Include(s => s.LanguageCaption)
                .ToListAsync();

            return data;
        }

        public async Task Update(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Session>> GetByMovieId(int id)
        {
            var data = await _context.Sessions
                .Where(s => s.MovieId == id && s.SessionHour.Date >= DateTime.Today)
                .Include(s => s.Movie)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                .Include(s => s.LanguageCaption)
                .Include(s => s.CinemaRoom!.Cinema)
                    .ThenInclude(c => c!.Address)
                .ToListAsync();
                
            return data;
        }

        public async Task<List<DateOnly>> GetAvailableDaysForMovie(int movieId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Sessions
                .Where(s => s.MovieId == movieId && DateOnly.FromDateTime(s.SessionHour) >= today)
                .Select(s => DateOnly.FromDateTime(s.SessionHour))
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        public async Task<List<Session>?> GetActiveSessionsByCity(string city)
        {
            var data = await _context.Sessions
                .Where(s => s.SessionHour > DateTime.Now &&
                            s.SessionHour.Date == DateTime.Today &&
                            s.CinemaRoom!.Cinema!.Address!.City!.ToLower() == city.ToLower())
                .Include(s => s.Movie)
                .Include(s => s.LanguageAudio)
                .Include(s => s.CinemaRoom)
                .Include(s => s.LanguageCaption)
                .ToListAsync();
            return data;
        }
    }
}
