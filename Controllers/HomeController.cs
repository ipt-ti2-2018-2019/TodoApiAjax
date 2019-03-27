using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}