using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using X.PagedList;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class ClientBookingController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public ClientBookingController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index(int? page, DateTime? key)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            int sessionId = (int)HttpContext.Session.GetInt32("_clientid");
            if (sessionId != null)
            {
                IEnumerable<BookingViewModel> list = GetClientBookingRequest(sessionId);
                if (!String.IsNullOrEmpty(key.ToString()))
                {
                    list = list.Where(x => x.BookedAt.Date == key.Value.Date);
                }
                return View(list.Reverse().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Details(int id)
        {
            IEnumerable<BookingDetailViewModel> bookingDetails = GetClientBookingDetailsRequest(id);
            return View(bookingDetails);
        }

        private IEnumerable<BookingDetailViewModel> GetClientBookingDetailsRequest(int id)
        {
            IEnumerable<BookingDetailViewModel> bookingDetails = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/getallbybooking/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                bookingDetails = JsonConvert.DeserializeObject<IEnumerable<BookingDetailViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return bookingDetails;
        }

        private IEnumerable<BookingViewModel> GetClientBookingRequest(int id)
        {
            IEnumerable<BookingViewModel> bookings = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"booking/getallbyuser/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                bookings = JsonConvert.DeserializeObject<IEnumerable<BookingViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return bookings;
        }
    }
}
