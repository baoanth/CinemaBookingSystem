using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
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

        public IActionResult UpdateProfile()
        {
            string ? sessionName = HttpContext.Session.GetString("_clientname");
            if (String.IsNullOrEmpty(sessionName))
            {
                HttpContext.Response.Redirect("/Account/Login");
                _notyf.Error("Bạn cần đăng nhập để thực hiện thao tác này");
                return Redirect("/Home/Error/401");
            }
            else
            {
                UserViewModel user = null;
                HttpResponseMessage response = GetUserDetails(sessionName);
                if (response.IsSuccessStatusCode)
                {
                    string body = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UserViewModel>(body);
                }
                else
                {
                    _notyf.Error("Không thể lấy thông tin do lỗi server");
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    return Redirect("/Home/Error/500");
                }
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(UserViewModel user)
        {
            HttpResponseMessage response = UpdateUserProfile(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Cập nhật thành công", 3);
                HttpContext.Session.SetString("_clientname", user.FullName);
                return RedirectToAction("Index","Home");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return View(user);
            }
        }

        public IActionResult ChangePassword()
        {
            string? sessionName = HttpContext.Session.GetString("_clientname");
            if (String.IsNullOrEmpty(sessionName))
            {
                HttpContext.Response.Redirect("/Account/Login");
                _notyf.Error("Bạn cần đăng nhập để thực hiện thao tác này");
                return Redirect("/Home/Error/401");
            }
            else
            {
                ViewBag.Username = sessionName;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword([Bind("Username,OldPassword,ConfirmPassword,NewPassword")] ChangePasswordViewModel changes)
        {
            if (changes.NewPassword != changes.ConfirmPassword)
            {
                _notyf.Error("Nhập lại mật khẩu mới không trùng khớp", 4);
            }
            else
            {
                HttpResponseMessage response = UpdateUserPassword(changes);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Đổi mật khẩu thành công, hãy tiến hành đăng nhập lại bằng mật khẩu mới", 3);
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    _notyf.Error("Mật khẩu cũ không chính xác", 4);
                    Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            return View(changes);
        }

        private HttpResponseMessage GetUserDetails(string name)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getbyusername?username={name}");
            request.Method = HttpMethod.Get;
            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateUserProfile(UserViewModel user)
        {
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
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

            request.Content = content;

            return _client.SendAsync(request).Result;
        }
    }
}
