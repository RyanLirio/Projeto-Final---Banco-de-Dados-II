using Cine_Ma.Data;
using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Repository
{
    public class SexMovieRepository : ISexMovieRepository
    {
        private readonly CineContext _context;

        public SexMovieRepository(CineContext movieContext)
        {
            _context = movieContext;
        }

        // --- MÉTODOS DE MANIPULAÇÃO DE DADOS (CUD) ---

        public async Task Create(SexMovie genderMovie)
        {
            await _context.SexMovies.AddAsync(genderMovie);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? originalSexId, int? originalMovieId, SexMovie genderMovieNewData)
        {
            var genderMovieOld = await _context.SexMovies
                .FindAsync(originalSexId, originalMovieId);

            if (genderMovieOld != null)
            {
                // Remove o registro antigo
                _context.SexMovies.Remove(genderMovieOld);
                await _context.SaveChangesAsync();

                // Adiciona o novo registro com os dados atualizados
                await _context.SexMovies.AddAsync(genderMovieNewData);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(SexMovie genderMovie)
        {
            _context.SexMovies.Remove(genderMovie);
            await _context.SaveChangesAsync();
        }

        // --- MÉTODOS DE CONSULTA (READ) ---

        public async Task<SexMovie?> Get(int ticketId, int orderId)
        {
            return await _context.SexMovies
                .Include(x => x.Sex)
                .Include(x => x.Movie)
                .FirstOrDefaultAsync(w => w.SexId == ticketId && w.SexId == orderId);
        }

        public async Task<List<SexMovie>> GetAll()
        {
            return await _context.SexMovies
                .Include(x => x.Sex)
                .Include(x => x.Movie)
                .ToListAsync();
        }

        public async Task<List<SexMovie>> GetByMovieId(int orderId)
        {
            return await _context.SexMovies
                .Include(x => x.Sex)
                .Include(x => x.Movie)
                .Where(w => w.MovieId == orderId)
                .ToListAsync();
        }

        public async Task<List<SexMovie>> GetBySexId(int ticketId)
        {
            return await _context.SexMovies
                .Include(x => x.Sex)
                .Include(x => x.Movie)
                .Where(w => w.SexId == ticketId)
                .ToListAsync();
        }
    }
}