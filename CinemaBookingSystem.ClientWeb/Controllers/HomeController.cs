using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.ClientWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
