using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
