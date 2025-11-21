using Cine_Ma.Models;
using Cine_Ma.Repository;
using Cine_Ma.ViewModels.Movies;
using CineMa.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ISexRepository _sexRepository;
        private readonly ISexMovieRepository _sexMovieRepository;
        private readonly ISessionRepository _sessionRepository;

        public MovieController(
            IMovieRepository movieRepository,
            ILanguageRepository languageRepository,
            ISexRepository sexRepository,
            ISexMovieRepository sexMovieRepository,
            ISessionRepository sessionRepository)
        {
            _movieRepository = movieRepository;
            _languageRepository = languageRepository;
            _sexRepository = sexRepository;
            _sexMovieRepository = sexMovieRepository;
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id, DateOnly? day, string? city)
        {
            

            var movie = await _movieRepository.GetById(id);
            var session = await _sessionRepository.GetByMovieId(id);
            var SessionDays = await _sessionRepository.GetAvailableDaysForMovie(id);

            city ??= session.First().CinemaRoom.Cinema.Address.City;

            var days = session
                .Select(s => DateOnly.FromDateTime(s.SessionHour))
                .Where(date => date >= DateOnly.FromDateTime(DateTime.Today))
                .Distinct()
                .OrderBy(d => d)
                .ToList();


            var selectedDay = day ?? days.First();



            var sessionsFilteredByDay = session
                .Where(s =>
                    DateOnly.FromDateTime(s.SessionHour) == selectedDay &&
                    s.CinemaRoom.Cinema.Address.City == city &&
                    s.SessionHour > DateTime.Now &&
                    s.SessionHour.Date >= DateTime.Today
                )
                .OrderBy(s => s.SessionHour)
                .ToList();

            var vm = new MovieSessionDetailsViewModel
            {
                SelectedCity = city,
                SelectedDay = selectedDay,
                DaysWithSession = SessionDays,
                Sessions = sessionsFilteredByDay ?? new List<Session>(),
                Movie = movie
            };

            return View(vm);
        }



        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            var data = await _movieRepository.GetAll();
            return View("~/Views/Admin/Movie/Index.cshtml", data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new MovieViewModel();

            vm.Languages = await _languageRepository.GetAll();

            vm.SetSexes(
                await _sexRepository.GetAll()
            );

            vm.DtRelease = DateOnly.FromDateTime(DateTime.Today);

            return View("~/Views/Admin/Movie/Create.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Languages = await _languageRepository.GetAll();

                var allSexes = await _sexRepository.GetAll();
                var selectedIds = vm.Sexes
                    .Where(s => s.IsSelected)
                    .Select(s => s.Id)
                    .ToHashSet();

                vm.Sexes = allSexes
                    .Select(s => new SelectedSex
                    {
                        Id = s.Id,
                        Name = s.Name!,
                        IsSelected = selectedIds.Contains(s.Id)
                    })
                    .ToList();

                return View("~/Views/Admin/Movie/Create.cshtml", vm);
            }

            var movie = new Movie
            {
                Title = vm.Title,
                Duration = vm.Duration,
                MinimumAge = vm.MinimumAge,
                Rating = vm.Rating,
                Description = vm.Description,
                Synopsis = vm.Synopsis,
                Studio = vm.Studio,
                DtRelease = vm.DtRelease,
                LanguageId = vm.LanguageId
            };

            await _movieRepository.Create(movie);

            foreach (var sex in vm.Sexes.Where(s => s.IsSelected))
            {
                await _sexMovieRepository.Create(
                    new SexMovie
                    {
                        MovieId = movie.Id,
                        SexId = sex.Id
                    }
                );
            }

            return RedirectToAction("IndexAdmin");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
                return NotFound();

            var vm = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Duration = movie.Duration,
                MinimumAge = movie.MinimumAge,
                Rating = movie.Rating,
                Description = movie.Description,
                Synopsis = movie.Synopsis,
                Studio = movie.Studio,
                DtRelease = movie.DtRelease,
                LanguageId = movie.LanguageId
            };

            vm.Languages = await _languageRepository.GetAll();

            var allSexes = await _sexRepository.GetAll();
            vm.SetSexes(allSexes);

            var movieSexRelations = await _sexMovieRepository.GetByMovieId(id);
            var selectedSexIds = movieSexRelations
                .Select(ms => ms.SexId)
                .ToHashSet();

            foreach (var sexVm in vm.Sexes)
            {
                if (selectedSexIds.Contains(sexVm.Id))
                    sexVm.IsSelected = true;
            }

            return View("~/Views/Admin/Movie/Update.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, MovieViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Languages = await _languageRepository.GetAll();

                var allSexes = await _sexRepository.GetAll();
                var selectedIds = vm.Sexes
                    .Where(s => s.IsSelected)
                    .Select(s => s.Id)
                    .ToHashSet();

                vm.Sexes = allSexes
                    .Select(s => new SelectedSex
                    {
                        Id = s.Id,
                        Name = s.Name!,
                        IsSelected = selectedIds.Contains(s.Id)
                    })
                    .ToList();

                return View("~/Views/Admin/Movie/Update.cshtml", vm);
            }

            var movie = await _movieRepository.GetById(id);
            if (movie == null)
                return NotFound();

            movie.Title = vm.Title;
            movie.Duration = vm.Duration;
            movie.MinimumAge = vm.MinimumAge;
            movie.Rating = vm.Rating;
            movie.Description = vm.Description;
            movie.Synopsis = vm.Synopsis;
            movie.Studio = vm.Studio;
            movie.DtRelease = vm.DtRelease;
            movie.LanguageId = vm.LanguageId;

            await _movieRepository.Update(movie);

            var existingRelations = await _sexMovieRepository.GetByMovieId(id);

            foreach (var rel in existingRelations)
            {
                await _sexMovieRepository.Delete(rel);
            }

            foreach (var sex in vm.Sexes.Where(s => s.IsSelected))
            {
                await _sexMovieRepository.Create(new SexMovie
                {
                    MovieId = movie.Id,
                    SexId = sex.Id
                });
            }

            return RedirectToAction("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieSexRelations = await _sexMovieRepository.GetByMovieId(id);

            foreach (var rel in movieSexRelations)
            {
                await _sexMovieRepository.Delete(rel);
            }

            await _movieRepository.Delete(movie);

            return RedirectToAction("IndexAdmin");
        }

        public async Task<IActionResult> Movie(int id)
        {
            var data = await _sessionRepository.GetById(id);

            return View(data);
        }
    }
}
