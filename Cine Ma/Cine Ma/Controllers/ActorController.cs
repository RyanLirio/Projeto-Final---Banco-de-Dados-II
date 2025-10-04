using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controller
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
