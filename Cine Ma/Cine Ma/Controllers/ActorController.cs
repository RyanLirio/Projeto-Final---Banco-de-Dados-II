using Cine_Ma.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class ActorController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public ActorController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
