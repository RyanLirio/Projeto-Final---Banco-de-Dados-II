using Cine_Ma.Data;
using Cine_Ma.Models;
using CineMa.Models.ViewModel;
using CineMa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineMa.Controllers
{
    public class AccountController : Controller
    {
        private readonly CineContext _context;
        private readonly FaceService _face;

        public AccountController(CineContext context, FaceService face)
        {
            _context = context;
            _face = face;
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

            return RedirectToAction("Index", "Home");
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

        // ==========================
        // CADASTRO DE ROSTO
        // ==========================
        public IActionResult RegisterFace() => View();

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
                ViewBag.Error = "Selecione uma foto.";
                return View();
            }

            using var ms = new MemoryStream();
            await faceImage.CopyToAsync(ms);

            var embedding = await _face.ExtractFeatures(ms.ToArray());

            if (embedding == null)
            {
                ViewBag.Error = "Nenhum rosto detectado ou falha na API.";
                return View();
            }

            var client = await _context.Clients.FindAsync(userId);
            client.FaceEmbedding = embedding;

            await _context.SaveChangesAsync();

            ViewBag.Success = "Rosto cadastrado com sucesso!";
            return View();
        }

        // ==========================
        // LOGIN FACIAL
        // ==========================
        public IActionResult LoginFacial() => View();

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

            var embeddingAtual = await _face.ExtractFeatures(ms.ToArray());

            if (embeddingAtual == null)
            {
                ViewBag.Erro = "Nenhum rosto detectado.";
                return View();
            }

            var clientes = _context.Clients.Where(c => c.FaceEmbedding != null).ToList();

            foreach (var c in clientes)
            {
                bool igual = await _face.CompareEmbeddings(embeddingAtual, c.FaceEmbedding);

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
