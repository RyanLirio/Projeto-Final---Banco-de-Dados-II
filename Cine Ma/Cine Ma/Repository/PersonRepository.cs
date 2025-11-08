/*/using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly CineContext _context;

        private static readonly List<Person> person = new List<Person>
        {
            new Person
            {
                Id = 1,
                Name = "Ryan Lirio",
                Gender = "Masculino",
                Birthday = new DateOnly(2005, 11, 4),
                Nationality = "Brazilian"
            }
        };

        public PersonRepository(CineContext context)
        {
            _context = context;
        }

        public async Task Create(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Person person)
        {
            _context.Persons.Remove(person);
            return _context.SaveChangesAsync();
        }

        public async Task<List<Person>> GetAll()
        {
            var data = await _context.Persons.ToListAsync();
            return data;
        }

        public async Task<Person?> GetById(int id)
        {
            var person = await _context.Persons.Where(p => p.Id == id).FirstOrDefaultAsync();
            return person;
        }

        
    }
}
*/