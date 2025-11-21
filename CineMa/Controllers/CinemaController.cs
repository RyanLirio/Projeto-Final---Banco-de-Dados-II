using Cine_Ma.Models;
using Cine_Ma.Repository;
using Cine_Ma.ViewModels.Cinemas;
using CineMa.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Controllers
{
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICinemaRoomRepository _roomRepository;
        private readonly IChairRepository _chairRepository;

        public CinemaController(
            ICinemaRepository cinemaRepository,
            IAddressRepository addressRepository,
            ICinemaRoomRepository roomRepository,
            IChairRepository chairRepository)
        {
            _cinemaRepository = cinemaRepository;
            _addressRepository = addressRepository;
            _roomRepository = roomRepository;
            _chairRepository = chairRepository;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var cinemas = await _cinemaRepository.GetAll();

            foreach (var c in cinemas)
            {
                c.Rooms = await _roomRepository.GetByCinemaId(c.Id);
            }

            return View("~/Views/Admin/Cinema/Index.cshtml", cinemas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var vm = new CinemaViewModel();
            return View("~/Views/Admin/Cinema/Create.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CinemaViewModel vm)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cinema/Create.cshtml", vm);

            try
            {
                var addr = new Address
                {
                    ZipCode = vm.Address.ZipCode,
                    City = vm.Address.City,
                    Road = vm.Address.Road,
                    State = vm.Address.State,
                    Number = vm.Address.Number,
                    Descripton = vm.Address.Descripton
                };

                await _addressRepository.Create(addr);

                var cinema = new Cinema
                {
                    Name = vm.Name,
                    Cnpj = vm.Cnpj,
                    Phone = vm.Phone,
                    Email = vm.Email,
                    AddressId = addr.Id
                };

                await _cinemaRepository.Create(cinema);

                return RedirectToAction("IndexAdmin");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    TempData["Erro"] = "CNPJ já cadastrado! Por favor, use outro CNPJ.";
                    return View("~/Views/Admin/Cinema/Create.cshtml", vm);
                }
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var cinema = await _cinemaRepository.GetById(id);
            if (cinema == null)
                return NotFound();

            var vm = new CinemaViewModel
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Cnpj = cinema.Cnpj,
                Phone = cinema.Phone,
                Email = cinema.Email,
                Address = new AddressViewModel
                {
                    Id = cinema.Address?.Id,
                    ZipCode = cinema.Address?.ZipCode,
                    City = cinema.Address?.City,
                    Road = cinema.Address?.Road,
                    State = cinema.Address?.State,
                    Number = cinema.Address?.Number ?? 0,
                    Descripton = cinema.Address?.Descripton
                }
            };

            return View("~/Views/Admin/Cinema/Update.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CinemaViewModel vm)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cinema/Update.cshtml", vm);

            try
            {
                var cinema = await _cinemaRepository.GetById(id);
                if (cinema == null)
                    return NotFound();

                cinema.Name = vm.Name;
                cinema.Cnpj = vm.Cnpj;
                cinema.Phone = vm.Phone;
                cinema.Email = vm.Email;

                cinema.Address!.ZipCode = vm.Address.ZipCode;
                cinema.Address.City = vm.Address.City;
                cinema.Address.Road = vm.Address.Road;
                cinema.Address.State = vm.Address.State;
                cinema.Address.Number = vm.Address.Number;
                cinema.Address.Descripton = vm.Address.Descripton;

                await _addressRepository.Update(cinema.Address);
                await _cinemaRepository.Update(cinema);

                return RedirectToAction("IndexAdmin");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    TempData["Erro"] = "CNPJ já cadastrado! Por favor, use outro CNPJ.";
                    return View("~/Views/Admin/Cinema/Update.cshtml", vm);
                }
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var cinema = await _cinemaRepository.GetById(id);
            if (cinema == null)
                return NotFound();

            var rooms = await _roomRepository.GetByCinemaId(id);

            foreach (var r in rooms)
            {
                var chair = await _chairRepository.GetByRoom(r.Id);

                foreach (var c in chair)
                    await _chairRepository.Delete(c);

                await _roomRepository.Delete(r);
            }


            await _cinemaRepository.Delete(cinema);

            if (cinema.Address != null)
                await _addressRepository.Delete(cinema.Address);

            return RedirectToAction("IndexAdmin");
        }
    }
}
