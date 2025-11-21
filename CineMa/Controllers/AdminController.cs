using CineMa.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CineMa.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (!AdminHelper.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
