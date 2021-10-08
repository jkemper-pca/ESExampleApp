using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESExampleApp.Controllers
{
    public class ESController : Controller
    {
        private readonly ILogger<ESController> _logger;
        private readonly IPersonRepository personRepository;

        public ESController(ILogger<ESController> logger, IPersonRepository personRepository)
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
            ViewBag.PreviousSearch = search;
            return View(result);
        }
    }
}
