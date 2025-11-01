using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CineContext _context;

        public async Task Create(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Movie?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task Update(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
