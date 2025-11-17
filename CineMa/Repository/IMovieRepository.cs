using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IMovieRepository
    {
        public Task Create(Movie movie);
        public Task Update(Movie movie);
        public Task Delete(Movie movie);
        public Task<List<Movie>> GetByName(string name);
        public Task<Movie?> GetById(int id);
        public Task<List<Movie>> GetAll();
        public Task<List<Movie>> GetBySexId(int sexId);
        public Task<List<Movie>> GetByRelease();
    }
}
