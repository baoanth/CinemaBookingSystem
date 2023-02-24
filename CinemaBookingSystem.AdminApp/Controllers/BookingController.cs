using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Text;
using X.PagedList;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class BookingController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public BookingController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index(int? page, string? key, DateTime? bookingdate)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            IEnumerable<BookingViewModel>? list = null;
            HttpResponseMessage response = GetBookingListRequest();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<BookingViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            if (!String.IsNullOrEmpty(key))
            {
                key = key.ToLower().Trim();
                list = list.Where(x => x.VerifyCode.ToLower().Trim().Contains(key));
            }
            if (!String.IsNullOrEmpty(bookingdate.ToString()))
            {
                list = list.Where(x => x.BookedAt.Date == bookingdate.Value.Date);
            }
            return View(list.Reverse().ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            IEnumerable<BookingDetailViewModel>? bookingDetail = null;
            HttpResponseMessage response = GetBookingDetailsRequest(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                bookingDetail = JsonConvert.DeserializeObject<IEnumerable<BookingDetailViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            }
            return View(bookingDetail);
        }

        public IActionResult Verify()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(string verifycode)
        {
            if (String.IsNullOrEmpty(verifycode))
            {
                _notyf.Error("Hãy nhập mã xác thực!", 4);
            }
            else
            {
                HttpResponseMessage response = GetBookingByCodeRequest(verifycode);
                if (response.IsSuccessStatusCode)
                {
                    string body = response.Content.ReadAsStringAsync().Result;
                    BookingDetailViewModel bookingDetail = JsonConvert.DeserializeObject<BookingDetailViewModel>(body);
                    return RedirectToAction("Details", "Booking", new { id = bookingDetail.BookingId });
                }
                else
                {
                    _notyf.Warning("Không tìm thấy đơn đặt vé nào", 4);
                }
            }
            return View();
        }

        //Response message
        public HttpResponseMessage GetBookingListRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "booking/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetBookingDetailsRequest(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/getallbybooking/{id}");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetBookingByCodeRequest(string code)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"booking/getbyverifycode/{code}");
            request.Method = HttpMethod.Get;
            return _client.SendAsync(request).Result;
        }
    }
}
