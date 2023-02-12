using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class ContactController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/contact");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public ContactController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            IEnumerable<ContactViewModel> contact = null;
            HttpResponseMessage response = GetContactDetails();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                contact = JsonConvert.DeserializeObject<IEnumerable<ContactViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(contact.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ContactViewModel contact)
        {
            contact.FacebookURL.Cast<string>();
            HttpResponseMessage response = UpdateContact(contact);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Cập nhật thành công", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin không hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Index");
            }
        }

        private HttpResponseMessage GetContactDetails()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        private HttpResponseMessage UpdateContact(ContactViewModel contact)
        {
            string data = JsonConvert.SerializeObject(contact);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }
    }
}