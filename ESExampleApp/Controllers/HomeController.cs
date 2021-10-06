using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESExampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPersonRepository personRepository;

        public HomeController(ILogger<HomeController> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            this.personRepository = personRepository;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(personRepository.Get());
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            var result = personRepository.Search(search);
            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Person person)
        {
            personRepository.Add(person);

            return RedirectToAction("Index", "Home", new { added = "true" });
        }
    }
}
