using Cine_Ma.Models;
using Cine_Ma.Repository;
using Cine_Ma.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICinemaRoomRepository _roomRepository;

        public SessionController(
            ISessionRepository sessionRepository,
            IMovieRepository movieRepository,
            ILanguageRepository languageRepository,
            ICinemaRoomRepository roomRepository)
        {
            _sessionRepository = sessionRepository;
            _movieRepository = movieRepository;
            _languageRepository = languageRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            var sessions = await _sessionRepository.GetAll();
            return View("~/Views/Admin/Session/Index.cshtml", sessions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new SessionViewModel
            {
                Movies = await _movieRepository.GetAll(),
                Rooms = await _roomRepository.GetAll(),
                AudioLanguages = await _languageRepository.GetAll(),
                CaptionLanguages = await _languageRepository.GetAll(),
                SessionHour = DateTime.Now.AddHours(1),
                Is3D = false
            };

            return View("~/Views/Admin/Session/Create.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SessionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Movies = await _movieRepository.GetAll();
                vm.Rooms = await _roomRepository.GetAll();
                vm.AudioLanguages = await _languageRepository.GetAll();
                vm.CaptionLanguages = await _languageRepository.GetAll();

                return View("~/Views/Admin/Session/Create.cshtml", vm);
            }

            var session = new Session
            {
                MovieId = vm.MovieId,
                RoomId = vm.RoomId,
                LanguageId = vm.LanguageId,
                CaptionId = vm.CaptionId,
                TicketPrice = vm.TicketPrice,
                SessionHour = vm.SessionHour,
                Is3D = vm.Is3D
            };

            await _sessionRepository.Create(session);

            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var session = await _sessionRepository.GetById(id);

            if (session == null)
                return NotFound();

            var vm = new SessionViewModel
            {
                Id = session.Id,
                MovieId = session.MovieId,
                RoomId = session.RoomId,
                LanguageId = session.LanguageId,
                CaptionId = session.CaptionId,
                TicketPrice = session.TicketPrice,
                SessionHour = session.SessionHour,
                Is3D = session.Is3D,

                Movies = await _movieRepository.GetAll(),
                Rooms = await _roomRepository.GetAll(),
                AudioLanguages = await _languageRepository.GetAll(),
                CaptionLanguages = await _languageRepository.GetAll()
            };

            return View("~/Views/Admin/Session/Update.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SessionViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Movies = await _movieRepository.GetAll();
                vm.Rooms = await _roomRepository.GetAll();
                vm.AudioLanguages = await _languageRepository.GetAll();
                vm.CaptionLanguages = await _languageRepository.GetAll();

                return View("~/Views/Admin/Session/Update.cshtml", vm);
            }

            var session = await _sessionRepository.GetById(id);

            if (session == null)
                return NotFound();

            session.MovieId = vm.MovieId;
            session.RoomId = vm.RoomId;
            session.LanguageId = vm.LanguageId;
            session.CaptionId = vm.CaptionId;
            session.TicketPrice = vm.TicketPrice;
            session.SessionHour = vm.SessionHour;
            session.Is3D = vm.Is3D;

            await _sessionRepository.Update(session);

            return RedirectToAction("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _sessionRepository.GetById(id);
            if (session == null)
                return NotFound();

            await _sessionRepository.Delete(session);

            return RedirectToAction("IndexAdmin");
        }
    }
}
