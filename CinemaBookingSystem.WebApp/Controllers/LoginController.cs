using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
