using Cine_Ma.Data;
using Cine_Ma.Models;
using Cine_Ma.Repository;
using CineMa.Helpers;
using CineMa.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineMa.Controllers
{
    public class AccountController : Controller
    {
        private readonly CineContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IChairRepository _chairRepository;

        public AccountController(CineContext context, IClientRepository clientRepository, IOrderRepository orderRepository, IChairRepository chairRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
            _orderRepository = orderRepository;
            _chairRepository = chairRepository;
        }

        // ==========================
        // LOGIN NORMAL
        // ==========================
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(password, client.SenhaHash))
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", client.Id);
            HttpContext.Session.SetString("UsuarioNome", client.Name);
            HttpContext.Session.SetString("UsuarioRole", client.Role ?? "User");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = await _clientRepository.GetById(userId.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Order(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderRepository.GetByIdClient(userId.Value, orderId);

            if (order == null)
            {
                return NotFound();
            }

            var firstTicket = order.Tickets?.FirstOrDefault();
            if (firstTicket?.Chair?.Room != null)
            {
                var roomId = firstTicket.Chair.Room.Id;
                var allChairs = await _chairRepository.GetByRoom(roomId);
                ViewBag.AllChairs = allChairs;
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // ==========================
        // REGISTRO
        // ==========================
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Registrar(RegisterClientViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var endereco = new Address
            {
                ZipCode = vm.ZipCode,
                City = vm.City,
                Road = vm.Road,
                State = vm.State,
                Number = vm.Number,
                Descripton = vm.Descripton
            };

            _context.Addresses.Add(endereco);
            await _context.SaveChangesAsync();

            var cliente = new Client
            {
                Name = vm.Name,
                Cpf = vm.Cpf,
                Email = vm.Email,
                Phone = vm.Phone,
                Birthday = vm.Birthday,
                RegistrationDate = DateTime.Now,
                AddressId = endereco.Id,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(vm.Senha)
            };

            _context.Clients.Add(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            var users = await _context.Clients.Include(c => c.Address).ToListAsync();
            return View("~/Views/Admin/User/Index.cshtml", users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            return View("~/Views/Admin/User/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterClientViewModel vm, string Role, string NewPassword)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View("~/Views/Admin/User/Create.cshtml", vm);

            var address = new Address
            {
                ZipCode = vm.ZipCode,
                City = vm.City,
                Road = vm.Road,
                State = vm.State,
                Number = vm.Number,
                Descripton = vm.Descripton
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var client = new Client
            {
                Name = vm.Name,
                Cpf = vm.Cpf,
                Email = vm.Email,
                Phone = vm.Phone,
                Birthday = vm.Birthday,
                RegistrationDate = DateTime.Now,
                AddressId = address.Id,
                Role = Role,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(NewPassword)
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return NotFound();

            var vm = new RegisterClientViewModel
            {
                Name = client.Name,
                Cpf = client.Cpf,
                Email = client.Email,
                Phone = client.Phone,
                Birthday = client.Birthday,
                ZipCode = client.Address.ZipCode,
                City = client.Address.City,
                Road = client.Address.Road,
                State = client.Address.State,
                Number = client.Address.Number,
                Descripton = client.Address.Descripton
            };

            ViewBag.Role = client.Role;

            return View("~/Views/Admin/User/Update.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RegisterClientViewModel vm, string Role, string NewPassword)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            ModelState.Remove("Senha");
            ModelState.Remove("NewPassword");

            if (!ModelState.IsValid)
                return View("~/Views/Admin/User/Update.cshtml", vm);

            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return NotFound();

            client.Name = vm.Name;
            client.Cpf = vm.Cpf;
            client.Email = vm.Email;
            client.Phone = vm.Phone;
            client.Birthday = vm.Birthday;
            client.Role = Role;

            client.Address.ZipCode = vm.ZipCode;
            client.Address.City = vm.City;
            client.Address.Road = vm.Road;
            client.Address.State = vm.State;
            client.Address.Number = vm.Number;
            client.Address.Descripton = vm.Descripton;

            if (!string.IsNullOrWhiteSpace(NewPassword))
                client.SenhaHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);

            await _context.SaveChangesAsync();

            return RedirectToAction("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AdminHelper.IsAdmin(HttpContext))
                return RedirectToAction("Index", "Home");

            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return NotFound();

            var address = client.Address;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexAdmin");
        }
    }
}