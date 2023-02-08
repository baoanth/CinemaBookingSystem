using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class SignupController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/account");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public SignupController(INotyfService notyf)
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
        public IActionResult Signup([Bind("UserId,Username,Password,FullName,DOB,Email,PhoneNumber,Address,RoleId")] UserViewModel user)
        {
            const int NORMAL_ROLE = 3;
            user.RoleId = NORMAL_ROLE;
            HttpResponseMessage response = SignupRequest(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Đăng ký thành công tài khoản: {user.Username}, hãy tiến hành đăng nhập bằng tài khoản mới!", 3);
                return RedirectToAction("Index", "Login");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(User);
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