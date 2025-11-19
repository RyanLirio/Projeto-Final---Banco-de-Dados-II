using Cine_Ma.Models;
using Cine_Ma.Repository;
using CineMa.Models;
using CineMa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CineMa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository MovieRepository;
        private readonly ISessionRepository SessionRepository;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository, ISessionRepository sessionRepository)
        {
            _logger = logger;
            MovieRepository = movieRepository;
            SessionRepository = sessionRepository;
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
            var movies = await MovieRepository.GetAll();

            //lançamentos
            var upcoming = await MovieRepository.GetByRelease();

            //sessoes ativas
            var sessions = await SessionRepository.GetActiveSessions();

            //filmes em cartaz
            var moviesInCartaz = upcoming
                .Where(m => sessions.Any(s => s.MovieId == m.Id))
                .Distinct()
                .ToList();

            var vm = new HomeViewModel
            {
                UpcomingReleases = upcoming ?? new List<Movie>(),
                MoviesInCartaz = moviesInCartaz ?? new List<Movie>()
            };

            return View(vm);
        }

    }
}
