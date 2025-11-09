using Cine_Ma.Models;

namespace Cine_Ma.Repository
{
    public interface IPersonRepository
    {
        public Task Create(Person person);
        public Task Update(Person person);
        public Task Delete(Person person);
        public Task<Person?> GetById(int id);
        public Task<List<Person>> GetAll();
    }
}
