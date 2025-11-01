using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IChairRepository
    {
        public Task Create(Chair chair);
        public Task Update(Chair chair);
        public Task Delete(Chair chair); 
        public Task<List<Chair>?> GetByRow(string row, int roomId);
        public Task<List<Chair>?> GetByColumn(int column, int roomId);
        public Task<List<Chair>?> GetByRoom(int roomId);
        public Task<List<Chair>> GetAll();
    }
}
