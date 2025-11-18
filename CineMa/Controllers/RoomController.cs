using Cine_Ma.Models;
using Cine_Ma.Repository;
using Cine_Ma.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class RoomController : Controller
    {
        private readonly ICinemaRoomRepository _roomRepository;
        private readonly IChairRepository _chairRepository;

        public RoomController(
            ICinemaRoomRepository roomRepository,
            IChairRepository chairRepository)
        {
            _roomRepository = roomRepository;
            _chairRepository = chairRepository;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin(int roomId)
        {
            var room = await _roomRepository.GetById(roomId);
            if (room == null)
                return NotFound();

            var chairs = await _chairRepository.GetByRoom(roomId);

            ViewBag.Room = room;
            ViewBag.TotalRows = chairs.Select(c => c.Row).Distinct().Count();
            ViewBag.TotalColumns = chairs.Select(c => c.Column).Distinct().Count();

            return View("~/Views/Admin/Chair/Index.cshtml", chairs);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var vm = new CinemaRoomViewModel
            {
                CinemaId = id,
                TotalColumns = 5,
                TotalRows = 5
            };

            return View("~/Views/Admin/Room/Create.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CinemaRoomViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Room/Create.cshtml", vm);

            var room = new CinemaRoom
            {
                CinemaId = vm.CinemaId,
                RoomNumber = vm.RoomNumber
            };

            await _roomRepository.Create(room);

            for (int r = 1; r <= vm.TotalRows; r++)
            {
                string rowLetter = ((char)('A' + (r - 1))).ToString();

                for (int c = 1; c <= vm.TotalColumns; c++)
                {
                    await _chairRepository.Create(new Chair
                    {
                        RoomId = room.Id,
                        Column = c,
                        Row = rowLetter
                    });
                }
            }

            return RedirectToAction("IndexAdmin", "Cinema");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var room = await _roomRepository.GetById(id);

            if (room == null)
                return NotFound();

            var vm = new CinemaRoomViewModel
            {
                Id = room.Id,
                CinemaId = room.CinemaId,
                RoomNumber = room.RoomNumber,

                TotalRows = room.Chairs.Select(c => c.Row).Distinct().Count(),

                TotalColumns = room.Chairs.Select(c => c.Column).Distinct().Count(),

                Chairs = room.Chairs.Select(c => new ChairViewModel
                {
                    RoomId = room.Id,
                    Column = c.Column,
                    Row = c.Row
                }).ToList()
            };

            return View("~/Views/Admin/Room/Update.cshtml", vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, CinemaRoomViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Room/Update.cshtml", vm);

            var room = await _roomRepository.GetById(id);
            if (room == null)
                return NotFound();

            room.RoomNumber = vm.RoomNumber;

            await _roomRepository.Update(room);

            var existingChairs = await _chairRepository.GetByRoom(room.Id);
            foreach (var c in existingChairs)
                await _chairRepository.Delete(c);

            for (int r = 1; r <= vm.TotalRows; r++)
            {
                string rowLetter = ((char)('A' + (r - 1))).ToString();

                for (int c = 1; c <= vm.TotalColumns; c++)
                {
                    await _chairRepository.Create(new Chair
                    {
                        RoomId = room.Id,
                        Column = c,
                        Row = rowLetter
                    });
                }
            }

            return RedirectToAction("IndexAdmin", "Cinema");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomRepository.GetById(id);

            if (room == null)
                return NotFound();

            foreach (var c in room.Chairs)
                await _chairRepository.Delete(c);

            await _roomRepository.Delete(room);

            return RedirectToAction("IndexAdmin", "Cinema");
        }
    }
}
