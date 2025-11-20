using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CineContext _context;

        public MovieRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Movie>> GetAll()
        {
            var data = await _context.Movies
                .Include(m => m.Language)
                .Include(m => m.SexMovies)
                    .ThenInclude(sm => sm.Sex)
                .ToListAsync();
            return data;
        }

        public async Task<Movie?> GetById(int id)
        {
            var movie = await _context.Movies
                .Where(p => p.Id == id)
                .Include(m => m.Language)
                .Include(m => m.SexMovies)
                .ThenInclude(sm => sm.Sex)
                .FirstOrDefaultAsync();
            return movie;
        }

        public async Task<List<Movie>> GetByName(string name)
        {
            var movies = await _context.Movies
                .Where(m => m.Title!.ToLower()
                .Contains(name.ToLower()))
                .Include(m => m.Language)
                .ToListAsync();

            return movies;
        }

        public async Task<List<Movie>> GetByRelease()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly in30Days = today.AddDays(30);

            var movies = await _context.Movies
                .Where(m => m.DtRelease >= today && m.DtRelease <= in30Days)
                .Include(m => m.Language)              // ok
                .Include(m => m.SexMovies)             // ok
                    .ThenInclude(sm => sm.Sex)         // ok
                .ToListAsync();

            return movies;
        }


        public async Task<List<Movie>> GetBySexId(int sexId)
        {
            var movies = await _context.SexMovies
                .Where(m => m.SexId == sexId)
                .Select(m => m.Movie!)
                .ToListAsync();
            return movies;
        }

    }
}
