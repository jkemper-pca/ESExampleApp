using ESExampleApp.Core;
using ESExampleApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            var stopwatch = Stopwatch.StartNew();
            var result = personRepository.ESSearch(search);
            stopwatch.Stop();
            ViewBag.PreviousSearch = search;
            ViewBag.ExecutionTime = stopwatch.Elapsed;
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
            var result = personRepository.ESBigSearch(search);
            stopwatch.Stop();
            ViewBag.PreviousSearch = search;
            ViewBag.ExecutionTime = stopwatch.Elapsed;
            return View(result);
        }

    }
}
