using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Diagnostics;
using System.Reflection;

namespace CinemaBookingSystem.AdminApp.Identify
{
    [Route("login")]
    public class LoginController : Controller
    {
        Uri _baseUrl = new Uri("https://localhost:44322/api/account");
        HttpClient _client;
        private const string APIKEY = "movienew";
        public const string SessionId = "_id";
        public const string SessionKeyName = "_name";
        public const string SessionFullName = "_fullname";
        public const string SessionKeyRole = "_role";
        public const string SessionRoleName = "_rolename";
        private INotyfService _notyf;

        public LoginController(INotyfService notyf)
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
            string data = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/systemlogin");
            request.Method = HttpMethod.Post;
            
            request.Content = content;

            HttpResponseMessage response = _client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                UserViewModel user = null;
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
                _notyf.Success($"Chào mừng quay trở lại, {user.FullName}", 4);
                HttpContext.Session.SetInt32(SessionId, user.UserId);
                HttpContext.Session.SetString(SessionKeyName, user.Username);
                HttpContext.Session.SetString(SessionFullName, user.FullName);
                HttpContext.Session.SetInt32(SessionKeyRole, user.RoleId);
                HttpContext.Session.SetString(SessionRoleName, user.Role.RoleName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyf.Error("Tài khoản hoặc mật khẩu không khớp với xác thực hệ thống!", 4);
            }
            return View(login);
        }
    }
}