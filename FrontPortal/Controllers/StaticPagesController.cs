using System.Diagnostics;
using System.Net;
using Gitloy.Services.FrontPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.FrontPortal.Controllers
{
    [Route("/")]
    public class StaticPagesController : Controller
    {
        private readonly ILogger _logger;

        public StaticPagesController(ILogger<StaticPagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/Docs")]
        public IActionResult Docs()
        {
            ViewData["Message"] = "Your documentation page.";

            return View();
        }

        [HttpGet]
        [Route("/About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your about page.";

            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}