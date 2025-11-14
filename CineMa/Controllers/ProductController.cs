using Cine_Ma.Models;
using Cine_Ma.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin()
        {
            return View("~/Views/Admin/Product/Index.cshtml", await _productRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Product/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, string Price)
        {
            ModelState.Remove("Price");

            Price = Price.Replace(",", "");
            int price = Convert.ToInt32(Price);
            product.Price = price;

            if (ModelState.IsValid)
            {
                
                await _productRepository.Create(product);
                return RedirectToAction("IndexAdmin");
            }
            return View("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(product);
            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var product = await _productRepository.GetById(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Product/Update.cshtml", product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Product product, string Price)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            if (id.Value != product.Id)
            {
                return BadRequest();
            }

            ModelState.Remove("Price");

            Price = Price.Replace(",", "");
            int price = Convert.ToInt32(Price);
            product.Price = price;

            if (ModelState.IsValid)
            {
                await _productRepository.Update(product);
                return RedirectToAction("IndexAdmin");
            }

            return View("IndexAdmin", product);
        }
    }
}
