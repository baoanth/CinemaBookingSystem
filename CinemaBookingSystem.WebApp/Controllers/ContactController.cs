using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using CinemaBookingSystem.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;

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
        public async void SendContact(string fullname, string phonenumber, string email, string message)
        {
            string subject = "Phản hồi từ khách hàng " + fullname;
            const string receiveMailAddress = "huynhconghung131@gmail.com";
            message = "Số điện thoại khách hàng:" + phonenumber + ". " + message;
            using (SmtpClient client = new SmtpClient("localhost"))
            {
                if(await MailUtils.SendMail(email, receiveMailAddress, subject, message, client))
                {
                    _notyf.Success("Phản hồi đã được gửi! Chúng tôi xin cảm ơn vì những feedback của bạn!", 5);
                }
                else
                {
                    _notyf.Warning("Lỗi", 5);
                }
                
            }
            RedirectToAction("Index", "Contact");
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