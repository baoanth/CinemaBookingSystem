using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace CinemaBookingSystem.AdminApp.Identify
{
    [Route("login")]
    public class LoginController : Controller
    {
        Uri _baseUrl = new Uri("https://localhost:44322/api/account");
        HttpClient _client;
        private const string APIKEY = "movienew";
        public const string SessionKeyName = "_name";

        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginViewModel login)
        {
            string data = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/systemlogin");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            HttpResponseMessage response = _client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString(SessionKeyName, login.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin không hợp lệ!");
            }
            return View(login);
        }
    }
}