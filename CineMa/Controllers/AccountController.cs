using Cine_Ma.Data;
using Cine_Ma.Models;
using CineMa.Models.ViewModel;
using CineMa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineMa.Controllers
{
    public class AccountController : Controller
    {

        private readonly CineContext _context;
        private readonly FaceService _face;

        // ============================
        // ÚNICO CONSTRUTOR VÁLIDO
        // (Seu código tinha dois — mantive a lógica e uni os dois)
        // ============================
        public AccountController(CineContext context, FaceService face)
        {
            _context = context;
            _face = face;     // adicionado somente para login facial
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

        
        //daqui pra frente é só pra trás


        //CADASTRAR ROSTO
        [HttpGet]
        public IActionResult RegisterFace()
        {
            return View();
        }


        //CADASTRAR ROSTO
        [HttpPost]
        public async Task<IActionResult> RegisterFace(IFormFile faceImage)
        {
            if (!HttpContext.Session.TryGetValue("UsuarioId", out var userBytes))
            {
                ViewBag.Error = "Você precisa estar logado.";
                return View();
            }

            int userId = BitConverter.ToInt32(userBytes);

            if (faceImage == null || faceImage.Length == 0)
            {
                ViewBag.Error = "Selecione uma foto válida.";
                return View();
            }

            using var ms = new MemoryStream();
            await faceImage.CopyToAsync(ms);

            var faceId = await _face.DetectFace(ms.ToArray());

            if (faceId == null)
            {
                ViewBag.Error = "Nenhum rosto detectado.";
                return View();
            }

            var client = await _context.Clients.FindAsync(userId);
            client.PersonIdAzure = faceId;

            await _context.SaveChangesAsync();

            ViewBag.Success = "Rosto cadastrado com sucesso!";
            return View();
        }


 
        // LOGIN FACIAL
        [HttpGet]
        public IActionResult LoginFacial()
        {
            return View();
        }


        // LOGIN FACIAL
        [HttpPost]
        public async Task<IActionResult> LoginFacial(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ViewBag.Erro = "Envie uma foto.";
                return View();
            }

            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);

            var faceIdAtual = await _face.DetectFace(ms.ToArray());

            if (faceIdAtual == null)
            {
                ViewBag.Erro = "Nenhum rosto detectado.";
                return View();
            }

            var clientes = _context.Clients.ToList();

            foreach (var c in clientes)
            {
                if (string.IsNullOrEmpty(c.PersonIdAzure))
                    continue;

                bool igual = await _face.CompareFaces(faceIdAtual, c.PersonIdAzure);

                if (igual)
                {
                    HttpContext.Session.SetInt32("UsuarioId", c.Id);
                    HttpContext.Session.SetString("UsuarioNome", c.Name);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Erro = "Nenhum usuário correspondente.";
            return View();
        }
    }
}
