using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            var stopwatch = Stopwatch.StartNew();
            var result = personRepository.SQLSearch(search);
            stopwatch.Stop();
            ViewBag.ExecutionTime = stopwatch.Elapsed;
            ViewBag.PreviousSearch = search;
            return View(result);
        }

        [HttpGet]
        public IActionResult BigSearch()
        {
            return View(personRepository.Get());
        }

        [HttpPost]
        public IActionResult BigSearch(string search)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = personRepository.SQLBigSearch(search);
            stopwatch.Stop();
            ViewBag.PreviousSearch = search;
            ViewBag.ExecutionTime = stopwatch.Elapsed;
            return View(result);
        }

    }
}
