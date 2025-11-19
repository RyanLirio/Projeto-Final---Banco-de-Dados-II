using Cine_Ma.Models;
using Cine_Ma.Repository;
using CineMa.Models;
using CineMa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CineMa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository MovieRepository;
        private readonly ISessionRepository SessionRepository;
        private readonly IProductRepository ProductRepository;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository, ISessionRepository sessionRepository, IProductRepository productRepository)
        {
            _logger = logger;
            MovieRepository = movieRepository;
            SessionRepository = sessionRepository;
            ProductRepository = productRepository;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {

            var products = await ProductRepository.GetAll();

            var foods = products
                .Where(p => p.Category == "Comida")
                .ToList();

            var drinks = products
                .Where(p => p.Category == "Bebida")
                .ToList();
            
            var combos = products
                .Where(p => p.Category == "Combo")
                .ToList();


            var movies = await MovieRepository.GetAll();

            //lançamentos
            var upcoming = await MovieRepository.GetByRelease();

            //sessoes ativas
            var sessions = await SessionRepository.GetActiveSessions();

            //filmes em cartaz
            var moviesInCartaz = movies
                .Where(m => sessions.Any(s => s.MovieId == m.Id))
                .Distinct()
                .ToList();

            var sessionsToday = sessions
                .Where(s =>
                    s.SessionHour.Date == DateTime.Today &&
                    s.SessionHour > DateTime.Now)
                .OrderBy(s => s.SessionHour)
                .ToList();

            


            var vm = new HomeViewModel
            {
                Combos = combos ?? new List<Product>(),
                Drinks = drinks ?? new List<Product>(),
                Foods = foods ?? new List<Product>(),
                ActiveSessions = sessionsToday ?? new List<Session>(),
                UpcomingReleases = upcoming ?? new List<Movie>(),
                MoviesInCartaz = moviesInCartaz ?? new List<Movie>()
            };

            return View(vm);
        }

    }
}
