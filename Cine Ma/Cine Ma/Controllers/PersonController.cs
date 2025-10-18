using Cine_Ma.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cine_Ma.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
