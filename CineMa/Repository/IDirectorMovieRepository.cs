using Cine_Ma.Models;
namespace Cine_Ma.Repository
{
    public interface IDirectorMovieRepository
    {
        public Task Create(DirectorMovie directorMovie);
        public Task Update(int? originalDirectorId, int? originalMovieId, DirectorMovie directorMovieNewData);
        public Task Delete(DirectorMovie directorMovie);
        public Task<List<DirectorMovie?>> GetByDirectorId(int directorId);
        public Task<List<DirectorMovie?>> GetByMovieId(int movieId);
        public Task<DirectorMovie?> Get(int directorId, int movieId);
        public Task<List<DirectorMovie>> GetByMovieName(string name);
        public Task<List<DirectorMovie>> GetByDirectorName(string name);
        public Task<List<DirectorMovie>> GetAll();
    }
}