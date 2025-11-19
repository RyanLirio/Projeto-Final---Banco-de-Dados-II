using Cine_Ma.Models;

namespace CineMa.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Movie> UpcomingReleases { get; set; } = new(); // lançamentos futuros
        public List<Movie> MoviesInCartaz { get; set; } = new();   // filmes com sessão ativa
    }
}
