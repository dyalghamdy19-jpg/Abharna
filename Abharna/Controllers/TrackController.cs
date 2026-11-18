using Microsoft.AspNetCore.Mvc;

namespace Abharna.Controllers
{
    public class TrackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}