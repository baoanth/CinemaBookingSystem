using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
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
                SetSessionValues(user.UserId, user.Username, user.FullName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyf.Error("Tài khoản hoặc mật khẩu không khớp với xác thực hệ thống!", 4);
            }
            return View(login);
        }

        private void SetSessionValues(int userId, string username, string fullName)
        {
            HttpContext.Session.SetInt32(SessionId, userId);
            HttpContext.Session.SetString(SessionKeyName, username);
            HttpContext.Session.SetString(SessionFullName, fullName);
        }

        private HttpResponseMessage LoginRequest(LoginViewModel login)
        {
            string data = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "account/login");
            request.Method = HttpMethod.Post;
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
            request.RequestUri = new Uri(_baseUrl + "account/signup");
            request.Method = HttpMethod.Post;
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        [Authorize]
        [AllowAnonymous]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("SignupGoogle", "Account")
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignupGoogle()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            UserViewModel user = new UserViewModel()
            {
                Email = result.Principal.FindFirst(ClaimTypes.Email).Value,
                Username = result.Principal.FindFirst(ClaimTypes.Email).Value,
                FullName = result.Principal.FindFirst(ClaimTypes.Name).Value,
            };
            if (result.Succeeded)
            {
                if (CheckUserExistanceRequest(user.Username))
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(user);
            }
            else
            {
                return AccessDenied();
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public bool CheckUserExistanceRequest(string? username)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"user/getbyusername?username={username}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                UserViewModel user = JsonConvert.DeserializeObject<UserViewModel>(body);
                _notyf.Success($"Chào mừng quay trở lại, {user.FullName}", 4);
                SetSessionValues(user.UserId, user.Username, user.FullName);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}