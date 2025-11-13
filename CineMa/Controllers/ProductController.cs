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
        public IActionResult Create()
        {
            return View();
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
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
