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

            foreach (var vipRow in vm.VipRows)
            {
                int rowIndex = vipRow[0] - 'A' + 1;
                if (rowIndex > vm.TotalRows)
                {
                    ModelState.AddModelError("VipRows", $"A fileira {vipRow} não existe.");
                    return View("~/Views/Admin/Room/Create.cshtml", vm);
                }
            }

            var room = new CinemaRoom
            {
                CinemaId = vm.CinemaId,
                RoomNumber = vm.RoomNumber
            };

            await _roomRepository.Create(room);

            for (int r = 1; r <= vm.TotalRows; r++)
            {
                string rowLetter = ((char)('A' + (r - 1))).ToString();
                bool isVipRow = vm.VipRows.Contains(rowLetter);

                for (int c = 1; c <= vm.TotalColumns; c++)
                {
                    await _chairRepository.Create(new Chair
                    {
                        RoomId = room.Id,
                        Column = c,
                        Row = rowLetter,
                        IsVip = isVipRow
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

            var chairs = room.Chairs ?? new List<Chair>();

            var vm = new CinemaRoomViewModel
            {
                Id = room.Id,
                CinemaId = room.CinemaId,
                RoomNumber = room.RoomNumber,

                TotalRows = chairs.Select(c => c.Row).Distinct().Count(),
                TotalColumns = chairs.Select(c => c.Column).Distinct().Count(),

                Chairs = chairs.Select(c => new ChairViewModel
                {
                    RoomId = room.Id,
                    Column = c.Column,
                    Row = c.Row,
                    IsVip = c.IsVip
                }).ToList(),

                VipRows = chairs
                    .Where(c => c.IsVip)
                    .Select(c => c.Row!)
                    .Distinct()
                    .OrderBy(r => r)
                    .ToList()
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

            foreach (var vipRow in vm.VipRows)
            {
                int rowIndex = vipRow[0] - 'A' + 1;
                if (rowIndex > vm.TotalRows)
                {
                    ModelState.AddModelError("VipRows", $"A fileira {vipRow} não existe.");
                    return View("~/Views/Admin/Room/Update.cshtml", vm);
                }
            }

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
                bool isVipRow = vm.VipRows.Contains(rowLetter);

                for (int c = 1; c <= vm.TotalColumns; c++)
                {
                    await _chairRepository.Create(new Chair
                    {
                        RoomId = room.Id,
                        Column = c,
                        Row = rowLetter,
                        IsVip = isVipRow
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

            var chairs = room.Chairs?.ToList() ?? new List<Chair>();

            foreach (var c in chairs)
                await _chairRepository.Delete(c);

            await _roomRepository.Delete(room);

            return RedirectToAction("IndexAdmin", "Cinema");
        }
    }
}
