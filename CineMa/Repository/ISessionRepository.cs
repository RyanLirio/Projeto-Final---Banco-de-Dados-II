using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ISessionRepository
    {
        public Task Create(Session session);
        public Task Update(Session session);
        public Task Delete(Session session);
        public Task<Session?> GetById(int id);
        public Task<List<Session>?> GetActiveSessions();
        public Task<List<Session>> GetAll();
        public Task<List<Session>> GetByMovieId(int id);
        public Task<List<DateOnly>> GetAvailableDaysForMovie(int movieId);

    }
}
