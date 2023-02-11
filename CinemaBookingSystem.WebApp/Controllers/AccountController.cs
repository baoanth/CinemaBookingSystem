using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/account");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        public const string SessionId = "_clientid";
        public const string SessionKeyName = "_clientname";
        public const string SessionFullName = "_clientfullname";
        private INotyfService _notyf;

        public AccountController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Username,Password")] LoginViewModel login)
        {
            HttpResponseMessage response = LoginRequest(login);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                UserViewModel user = JsonConvert.DeserializeObject<UserViewModel>(body);
                _notyf.Success($"Chào mừng quay trở lại, {user.FullName}", 4);
                HttpContext.Session.SetInt32(SessionId, user.UserId);
                HttpContext.Session.SetString(SessionKeyName, user.Username);
                HttpContext.Session.SetString(SessionFullName, user.FullName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyf.Error("Tài khoản hoặc mật khẩu không khớp với xác thực hệ thống!", 4);
            }
            return View(login);
        }

        private HttpResponseMessage LoginRequest(LoginViewModel login)
        {
            string data = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/login");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup([Bind("UserId,Username,Password,FullName,DOB,Email,PhoneNumber,Address,RoleId")] UserViewModel user)
        {
            if (user.Username != user.Username.Trim())
            {
                _notyf.Error("Tài khoản không được chứa khoảng trống!", 4);
                return View(user);
            }
            if (!ModelState.IsValid)
            {
                _notyf.Error("Thông tin chưa hợp lệ!", 4);
                return View(user);
            }
            HttpResponseMessage response = SignupRequest(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Đăng ký thành công tài khoản: {user.Username}, hãy tiến hành đăng nhập bằng tài khoản mới!", 3);
                return RedirectToAction("Account", "Login");
            }
            else
            {
                _notyf.Error("Không thể đăng ký do lỗi server", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(user);
        }

        public HttpResponseMessage SignupRequest(UserViewModel User)
        {
            string data = JsonConvert.SerializeObject(User);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/signup");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
    }
}
