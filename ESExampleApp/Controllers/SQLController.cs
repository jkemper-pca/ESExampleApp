using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESExampleApp.Controllers
{
    public class SQLController : Controller
    {
        private readonly ILogger<SQLController> _logger;
        private readonly IPersonRepository personRepository;

        public SQLController(ILogger<SQLController> logger, IPersonRepository personRepository)
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
            var result = personRepository.SQLSearch(search);
            return View(result);
        }

    }
}
