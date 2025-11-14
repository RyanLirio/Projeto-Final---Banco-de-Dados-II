using Cine_Ma.Models;
using Cine_Ma.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _languageRepository.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            return View("~/Views/Admin/Language/Index.cshtml", await _languageRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Language/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Language language)
        {
            if (ModelState.IsValid)
            {

                await _languageRepository.Create(language);
                return RedirectToAction("IndexAdmin");
            }
            return View("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var language = await _languageRepository.GetById(id);
            if (language == null)
            {
                return NotFound();
            }

            await _languageRepository.Delete(language);
            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var language = await _languageRepository.GetById(id.Value);

            if (language == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Language/Update.cshtml", language);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Language language)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            if (id.Value != language.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _languageRepository.Update(language);
                return RedirectToAction("IndexAdmin");
            }

            return View(language);
        }
    }
}
