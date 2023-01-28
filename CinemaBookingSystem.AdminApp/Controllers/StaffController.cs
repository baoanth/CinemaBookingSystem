using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
