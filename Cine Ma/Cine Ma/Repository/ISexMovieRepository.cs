using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ISexMovieRepository
    {
        public Task Create(SexMovie genderMovie);
        public Task Update(int? originalSexId, int? originalMovieId, SexMovie genderMovieNewData);
        public Task Delete(SexMovie genderMovie);
        public Task<List<SexMovie?>> GetBySexId(int gendertId);
        public Task<List<SexMovie?>> GetByMovieId(int movieId);
        public Task<SexMovie?> Get(int gendertId, int movieId);
        public Task<List<SexMovie>> GetAll();
    }
}