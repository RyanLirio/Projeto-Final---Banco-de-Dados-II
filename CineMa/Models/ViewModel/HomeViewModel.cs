using Cine_Ma.Models;

namespace CineMa.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Movie> MoviesByCity { get; set; } = new();
        public List<Movie> UpcomingMoviesByCity { get; set; } = new();
        public List<Session> SessionsByCity { get; set; } = new();

        public string? SelectedCity { get; set; }
        public List<Address> City{ get; set; } = new();
        public List<Product> Combos { get; set; } = new();
        public List<Product> Drinks { get; set; } = new();
        public List<Product> Foods { get; set; } = new();
        public List<Session> ActiveSessions { get; set; } = new();
        public List<Movie> UpcomingReleases { get; set; } = new(); // lançamentos futuros
        public List<Movie> MoviesInCartaz { get; set; } = new();   // filmes com sessão ativa
    }
}
