using Cine_Ma.Models;
using Cine_Ma.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class SexController : Controller
    {
        private readonly ISexRepository _sexRepository;

        public SexController(ISexRepository sexRepository)
        {
            _sexRepository = sexRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _sexRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sex sex)
        {
            if (ModelState.IsValid)
            {

                await _sexRepository.Create(sex);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var sex = await _sexRepository.GetById(id);
            if (sex == null)
            {
                return NotFound();
            }

            await _sexRepository.Delete(sex);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var sex = await _sexRepository.GetById(id.Value);

            if (sex == null)
            {
                return NotFound();
            }

            return View(sex);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Sex sex)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            if (id.Value != sex.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _sexRepository.Update(sex);
                return RedirectToAction("Index");
            }

            return View(sex);
        }
    }
}
