using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore.Options;
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
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<BookingViewModel>? list = GetBookingListRequest();
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
            IEnumerable<BookingDetailViewModel> bookingDetail = GetBookingDetailsRequest(id);

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

        public IActionResult ExportTicket(int id)
        {
            IEnumerable<BookingDetailViewModel> bookingDetail = GetBookingDetailsRequest(id);
            var printpdf = new Rotativa.AspNetCore.ViewAsPdf()
            {
                ViewName = "Ticket",
                Model = bookingDetail,
                PageHeight = 130,
                PageWidth = 80,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins { Bottom = 0, Left = 0, Right = 0, Top = 0 },
            };
            return printpdf;
        }

        public IActionResult Ticket(int id)
        {
            IEnumerable<BookingDetailViewModel> bookingDetail = GetBookingDetailsRequest(id);
            return View(bookingDetail);
        }

        public IActionResult Delete(int id)
        {
            HttpResponseMessage bookingDetailsDel = DeleteBookingDetailRequest(id);
            if (bookingDetailsDel.IsSuccessStatusCode)
            {
                HttpResponseMessage bookingDel = DeleteBookingRequest(id);
                if (bookingDel.IsSuccessStatusCode)
                {
                    _notyf.Success("Xóa đơn đặt vé thành công", 4);
                    return RedirectToAction("Index", "Booking");
                }
            }
            _notyf.Error("Xóa không thành công!", 4);
            return RedirectToAction("Index", "Booking");
        }

        //Response message
        public IEnumerable<BookingViewModel> GetBookingListRequest()
        {
            IEnumerable<BookingViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "booking/getall");
            request.Method = HttpMethod.Get;
            HttpResponseMessage response = _client.SendAsync(request).Result;
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
            return list;
        }

        public IEnumerable<BookingDetailViewModel> GetBookingDetailsRequest(int? id)
        {
            IEnumerable<BookingDetailViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/getallbybooking/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<BookingDetailViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }

        public HttpResponseMessage GetBookingByCodeRequest(string code)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"booking/getbyverifycode/{code}");
            request.Method = HttpMethod.Get;
            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteBookingRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"booking/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteBookingDetailRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}