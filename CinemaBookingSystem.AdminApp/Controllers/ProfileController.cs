using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.AdminApp.Models;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class ProfileController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/user");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public ProfileController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            UserViewModel user = null;
            string sessionName = HttpContext.Session.GetString("_name");
            HttpResponseMessage response = GetUserDetails(sessionName);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel user)
        {
            HttpResponseMessage response = UpdateUserProfile(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Cập nhật thành công", 3);
                HttpContext.Session.SetString("_fullname", user.FullName);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ChangePassword()
        {
            string sessionName = HttpContext.Session.GetString("_name");
            ViewBag.Username = sessionName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword([Bind("Username,OldPassword,ConfirmPassword,NewPassword")] ChangePasswordViewModel changes)
        {
            if(changes.OldPassword != changes.ConfirmPassword)
            {
                _notyf.Warning("Nhập lại mật khẩu không trùng khớp", 4);
            }
            else
            {
                HttpResponseMessage response = UpdateUserPassword(changes);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Cập nhật thành công, hãy tiến hành đăng nhập lại bằng mật khẩu mới", 3);
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    _notyf.Error("Mật khẩu cũ không chính xác", 4);
                    Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            return View();
        }
        private HttpResponseMessage GetUserDetails(string name)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getbyusername?username={name}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage UpdateUserProfile(UserViewModel user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage UpdateUserPassword(ChangePasswordViewModel changes)
        {
            string data = JsonConvert.SerializeObject(changes);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/changepassword");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
    }
}
