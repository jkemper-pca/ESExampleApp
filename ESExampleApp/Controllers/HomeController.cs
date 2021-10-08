using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using ESExampleApp.Generator;
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
            var result = personRepository.ESSearch(search);
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

        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View(personRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Person p)
        {
            if (ModelState.IsValid)
            {
                personRepository.Edit(p);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Seed()
        {
            RecordGenerator.GenerateRecords(personRepository);


            return RedirectToAction("Index");
        }
    }
}
