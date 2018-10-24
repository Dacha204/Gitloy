using Microsoft.AspNetCore.Mvc;

namespace Gitloy.Services.WebhookAPI.Controllers
{
    [Route("/api/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"[WebhookAPI: {System.Environment.MachineName}] ItWorks");
        }
    }
}