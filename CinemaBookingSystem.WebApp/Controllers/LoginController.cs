using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/account");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        public const string SessionKeyName = "_clientname";
        public const string SessionFullName = "_clientfullname";
        private INotyfService _notyf;

        public LoginController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Username,Password")] LoginViewModel login)
        {
            string data = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/login");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            HttpResponseMessage response = _client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                UserViewModel user = null;
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
                _notyf.Success($"Chào mừng quay trở lại, {user.FullName}", 4);
                HttpContext.Session.SetString(SessionKeyName, user.Username);
                HttpContext.Session.SetString(SessionFullName, user.FullName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyf.Error("Tài khoản hoặc mật khẩu không khớp với xác thực hệ thống!", 4);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}