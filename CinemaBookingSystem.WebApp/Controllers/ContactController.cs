using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using CinemaBookingSystem.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class ContactController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
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
            IEnumerable<ContactViewModel> contact = GetContactRequest();
            return View(contact);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SendContact(string email, string fullname, string phonenumber, string message)
        {
            CustomerContactViewModel customerContactViewModel = new CustomerContactViewModel()
            {
                CustomerName = fullname,
                CustomerEmail = email,
                CustomerPhone = phonenumber,
                Content = message,
                SendedAt = DateTime.Now,
                Status = false
            };
            HttpResponseMessage response = SendContactRequest(customerContactViewModel);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Gửi phản hồi thành công, chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất!", 4);
                return RedirectToAction("Index", "Contact");
            }
            else
            {
                _notyf.Error("Hãy điền đầy đủ thông tin vào mẫu!", 4);
                return RedirectToAction("Index", "Contact");
            }
        }

        public HttpResponseMessage SendContactRequest(CustomerContactViewModel contact)
        {
            string data = JsonConvert.SerializeObject(contact);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "customercontact/create");
            request.Method = HttpMethod.Post;
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public IEnumerable<ContactViewModel> GetContactRequest()
        {
            IEnumerable<ContactViewModel>? list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "contact/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ContactViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }
    }
}