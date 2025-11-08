/*using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class DirectorMovieRepository : IDirectorMovieRepository
    {
        private readonly CineContext _context;

        public DirectorMovieRepository(CineContext movieContext)
        {
            _context = movieContext;
        }

        // --- MÉTODOS DE MANIPULAÇÃO DE DADOS (CUD) ---

        public async Task Create(DirectorMovie studentCourses)
        {
            await _context.DirectorMovies.AddAsync(studentCourses);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? originalPersonId, int? originalMovieId, DirectorMovie directorMovieNewData)
        {
            var directorMovieOld = await _context.DirectorMovies
                .FindAsync(originalPersonId, originalMovieId);

            if (directorMovieOld != null)
            {
                // Remove o registro antigo
                _context.DirectorMovies.Remove(directorMovieOld);
                await _context.SaveChangesAsync();
                
                // Adiciona o novo registro com os dados atualizados
                await _context.DirectorMovies.AddAsync(directorMovieNewData);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(DirectorMovie studentCourses)
        {
            _context.DirectorMovies.Remove(studentCourses);
            await _context.SaveChangesAsync();
        }

        // --- MÉTODOS DE CONSULTA (READ) ---

        public async Task<DirectorMovie?> Get(int personId, int movieId)
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .FirstOrDefaultAsync(w => w.PersonId == personId && w.PersonId == movieId);
        }

        public async Task<List<DirectorMovie>> GetAll()
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .ToListAsync();
        }

        public async Task<List<DirectorMovie>> GetByMovieId(int movieId)
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .Where(w => w.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<DirectorMovie>> GetByDirectorId(int personId)
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .Where(w => w.PersonId == personId)
                .ToListAsync();
        }

        public async Task<List<DirectorMovie>> GetByMovieName(string movieName)
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .Where(w => w.Movie != null && w.Movie.Title != null && w.Movie.Title.ToLower().Contains(movieName.ToLower()))
                .ToListAsync();
        }

        public async Task<List<DirectorMovie>> GetByDirectorName(string name)
        {
            return await _context.DirectorMovies
                .Include(x => x.Movie)
                .Include(x => x.Person)
                .Where(w => w.Person != null && w.Person.Name != null && w.Person.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
    }
}
*/