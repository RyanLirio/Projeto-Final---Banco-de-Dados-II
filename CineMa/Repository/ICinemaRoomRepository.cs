using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface ICinemaRoomRepository
    {
        public Task Create(CinemaRoom cinemaRoom);
        public Task Update(CinemaRoom cinemaRoom);
        public Task Delete(CinemaRoom cinemaRoom);
        public Task<CinemaRoom?> GetByCinemaId(int id);
        public Task<CinemaRoom?> GetById(int id);
        public Task<List<CinemaRoom>> GetAll();
    }
}