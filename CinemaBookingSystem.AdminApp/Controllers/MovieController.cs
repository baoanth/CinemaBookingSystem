using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
