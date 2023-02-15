using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using CinemaBookingSystem.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class BookingController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;
        private BookingViewModel _booking;
        private static List<ScreeningPositionViewModel> _seats;

        public BookingViewModel Booking
        {
            set { _booking = value; }
            get { return _booking; }
        }

        public static List<ScreeningPositionViewModel> Seats
        {
            set { _seats = value; }
            get { return _seats; }
        }

        public BookingController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult PositionsChoose(int id)
        {
            HttpContext.Session.SetInt32("_currentScreeningId", id);
            IEnumerable<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsListRequest(id);
            return View(screeningPositions);
        }

        [HttpPost]
        public IActionResult BookingConfirm(List<int> availSeats)
        {
            if (availSeats.Count == 0)
            {
                _notyf.Warning("Bạn hãy vui lòng chọn ghế trước khi xác nhận nhé.", 4);
                return RedirectToAction("PositionsChoose", "Booking", new { id = HttpContext.Session.GetInt32("_currentScreeningId") });
            }
            List<ScreeningPositionViewModel> pickedSeats = new List<ScreeningPositionViewModel>();
            for (int i = 0; i < availSeats.Count; i++)
            {
                ScreeningPositionViewModel seat = GetScreeningPositionsDetailsRequest(availSeats[i]);
                pickedSeats.Add(seat);
            }
            ViewBag.PaymentMethods = GetPaymentListRequest();
            Seats = pickedSeats;
            return View(pickedSeats);
        }

        public ActionResult Checkout(int paymentid, int total)
        {
            CreateBooking(paymentid);
            CreateBookingDetails(Booking, Seats);
            switch (paymentid)
            {
                case 4:
                    return MomoPayment(total);

                default:
                    return MomoPayment(total);
            }
        }

        private void CreateBookingDetails(BookingViewModel booking, IEnumerable<ScreeningPositionViewModel> seats)
        {
            foreach (var item in seats)
            {
                BookingDetailViewModel bookingDetail = new BookingDetailViewModel()
                {
                    BookingId = booking.BookingId,
                    PositionId = item.PositionId,
                };
                CreateBookingDetailRequest(bookingDetail);
            }
        }

        public void CreateBooking(int paymentId)
        {
            BookingViewModel booking = new BookingViewModel()
            {
                IsPayed = false,
                UserId = (int)HttpContext.Session.GetInt32("_clientid"),
                PaymentId = paymentId,
                BookedAt = DateTime.Now,
            };
            HttpResponseMessage response = CreateBookingRequest(booking);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                Booking = JsonConvert.DeserializeObject<BookingViewModel>(body);
                HttpContext.Session.SetInt32("_bookingid", Booking.BookingId);
            }
        }

        #region MOMO PAYMENT

        public ActionResult MomoPayment(int total)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán đặt vé tại rạp Cinemax";
            string returnUrl = $"https://localhost:7094/Booking/ConfirmPaymentClient";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = total.ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
            notifyurl + "&extraData=" +
            extraData;

            MomoSecurity crypto = new MomoSecurity();
            //sign signature SHA256
            string signature = crypto.SignSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            string responseFromMomo = MomoPaymentRequest.SendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult ConfirmPaymentClient(int bookingId)
        {
            //hiển thị thông báo cho người dùng
            string errorCode = HttpContext.Request.Query["errorCode"].ToString();
            if (errorCode == "0")
            {
                SavePayment();
                return View();
            }
            else
            {
                DeleteBooking();
                return RedirectToAction("PaymentFailed", "Booking");
            }
        }

        public ActionResult PaymentFailed()
        {
            return View();
        }

        public void SavePayment()
        {
            int bookingId = (int)HttpContext.Session.GetInt32("_bookingid");
            if (bookingId != null)
            {
                BookingViewModel booking = GetSingleBookingRequest(bookingId);
                booking.IsPayed = true;
                UpdateBookingRequest(booking);

                IEnumerable<BookingDetailViewModel> bookingDetails = GetBookingDetailRequest(bookingId);
                foreach (var position in bookingDetails)
                {
                    ScreeningPositionViewModel screeningPosition = GetScreeningPositionsDetailsRequest(position.PositionId);
                    screeningPosition.IsBooked = true;
                    UpdateScreeningPositionsRequest(screeningPosition);
                }
            }
        }

        private void DeleteBooking()
        {
            int bookingId = (int)HttpContext.Session.GetInt32("_bookingid");
            if (bookingId != null)
            {
                DeleteBookingDetailRequest(bookingId);
                DeleteBookingRequest(bookingId);
            }
        }

        #endregion MOMO PAYMENT

        public IEnumerable<BookingDetailViewModel> GetBookingDetailRequest(int bookingId)
        {
            IEnumerable<BookingDetailViewModel>? bookingDetail = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/getallbybooking/{bookingId}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                bookingDetail = JsonConvert.DeserializeObject<IEnumerable<BookingDetailViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return bookingDetail;
        }

        public HttpResponseMessage CreateBookingDetailRequest(BookingDetailViewModel bookingDetail)
        {
            string data = JsonConvert.SerializeObject(bookingDetail);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "bookingdetail/create");
            request.Method = HttpMethod.Post;
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage CreateBookingRequest(BookingViewModel booking)
        {
            string data = JsonConvert.SerializeObject(booking);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "booking/create");
            request.Method = HttpMethod.Post;
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateBookingRequest(BookingViewModel booking)
        {
            string data = JsonConvert.SerializeObject(booking);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "booking/update");
            request.Method = HttpMethod.Post;
            request.Content = content;

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

        public HttpResponseMessage UpdateScreeningPositionsRequest(ScreeningPositionViewModel screeningPosition)
        {
            string data = JsonConvert.SerializeObject(screeningPosition);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "screeningposition/update");
            request.Method = HttpMethod.Post;
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        private BookingViewModel GetSingleBookingRequest(int id)
        {
            BookingViewModel booking = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"booking/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                booking = JsonConvert.DeserializeObject<BookingViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return booking;
        }

        public IEnumerable<PaymentViewModel> GetPaymentListRequest()
        {
            IEnumerable<PaymentViewModel>? payments = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"payment/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                payments = JsonConvert.DeserializeObject<IEnumerable<PaymentViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return payments;
        }

        public IEnumerable<ScreeningPositionViewModel> GetScreeningPositionsListRequest(int? id)
        {
            IEnumerable<ScreeningPositionViewModel>? screeningPositions = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"screeningposition/getallbyscreening/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                screeningPositions = JsonConvert.DeserializeObject<IEnumerable<ScreeningPositionViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return screeningPositions;
        }

        public ScreeningPositionViewModel GetScreeningPositionsDetailsRequest(int? id)
        {
            ScreeningPositionViewModel? screeningPosition = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"screeningposition/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                screeningPosition = JsonConvert.DeserializeObject<ScreeningPositionViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return screeningPosition;
        }
    }
}