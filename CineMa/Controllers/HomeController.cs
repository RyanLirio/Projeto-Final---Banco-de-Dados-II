using System.Diagnostics;
using Cine_Ma.Repository;
using CineMa.Models;
using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository MovieRepository;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            MovieRepository = movieRepository;
        }


        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> FutureReleases()
        {
            var movie = await MovieRepository.GetByRelease();
            return View(movie ?? new List<Cine_Ma.Models.Movie>());
        }
    }
}
