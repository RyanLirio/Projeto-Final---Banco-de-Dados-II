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
            var data = await _context.Movies.ToListAsync();
            return data;
        }

        public async Task<Movie?> GetById(int id)
        {
            var movie = await _context.Movies.Where(p => p.Id == id).FirstOrDefaultAsync();
            return movie;
        }

        public async Task<List<Movie>> GetByName(string name)
        {
            var movies = await _context.Movies.Where(m => m.Title!.ToLower().Contains(name.ToLower())).ToListAsync();

            return movies;
        }
    }
}
