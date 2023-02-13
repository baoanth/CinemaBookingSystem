using CinemaBookingSystem.AdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("_name") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                HttpContext.Session.Remove("_name");
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}