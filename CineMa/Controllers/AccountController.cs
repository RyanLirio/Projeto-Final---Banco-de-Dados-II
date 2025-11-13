using Cine_Ma.Data;
using Cine_Ma.Models;
using CineMa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace CineMa.Controllers
{
    public class AccountController : Controller
    {

        private readonly CineContext _context;

        public AccountController(CineContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            //Procurar usuário pelo email
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == email);

            if (client == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            //Verificar se a senha está correta
            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(password, client.SenhaHash);

            if (!senhaCorreta)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            //Criar sessão
            HttpContext.Session.SetInt32("UsuarioId", client.Id);
            HttpContext.Session.SetString("UsuarioNome", client.Name);

            //Redirecionar após login
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            //apaga a sessão do usuário
            HttpContext.Session.Clear();

            //redireciona para a tela de login
            return RedirectToAction("Login", "Account");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegisterClientViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //Criar o endereço (Address)
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
            await _context.SaveChangesAsync(); // gera o Address.Id

            //Criar o cliente (Client) com AddressId
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
    }
}
