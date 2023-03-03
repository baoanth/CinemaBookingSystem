using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using CinemaBookingSystem.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml.Schema;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class BookingController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        private static List<ScreeningPositionViewModel> _seats;

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

        public IActionResult SeatPick(int id)
        {
            IEnumerable<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsListRequest(id);
            return View(screeningPositions);
        }

        [HttpPost]
        public IActionResult Confirm(List<int> chosenSeatIds)
        {
            //zero seat check
            if (chosenSeatIds.Count == 0)
            {
                _notyf.Warning("Bạn hãy vui lòng chọn ghế trước khi xác nhận nhé.", 4);
                return Redirect(Request.Headers["Referer"].ToString());
            }

            //store picked seats
            List<ScreeningPositionViewModel> pickedSeats = new List<ScreeningPositionViewModel>();

            for (int i = 0; i < chosenSeatIds.Count; i++)
            {
                ScreeningPositionViewModel seat = GetScreeningPositionsDetailsRequest(chosenSeatIds[i]);
                pickedSeats.Add(seat);
            }

            Seats = pickedSeats;

            ViewBag.PaymentMethods = GetPaymentListRequest();
            return View(pickedSeats);
        }

        public IActionResult Checkout(int paymentid, int total)
        {
            switch (paymentid)
            {
                case 4:
                    CreateBooking(paymentid, Seats);
                    return MomoPayment(total);
                case 5:
                    CreateBooking(paymentid, Seats);
                    return VnPayPayment(total);
                default:
                    _notyf.Information("Chức năng hiện đang phát triển nên chưa thể sử dụng!");
                    List<int> seats = new List<int>();
                    foreach (var item in Seats)
                    {
                        seats.Add(item.PositionId);
                    }
                    return RedirectToAction("Confirm", seats);
            }
        }

        public void CreateBooking(int paymentId, IEnumerable<ScreeningPositionViewModel> seats)
        {
            BookingViewModel bookingVm = new BookingViewModel()
            {
                IsPaid = false,
                UserId = (int)HttpContext.Session.GetInt32("_clientid"),
                PaymentId = paymentId,
                BookedAt = DateTime.Now,
            };
            HttpResponseMessage response = CreateBookingRequest(bookingVm);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                BookingViewModel booking = JsonConvert.DeserializeObject<BookingViewModel>(body);
                foreach (var item in seats)
                {
                    BookingDetailViewModel bookingDetail = new BookingDetailViewModel()
                    {
                        BookingId = booking.BookingId,
                        PositionId = item.PositionId,
                    };
                    CreateBookingDetailRequest(bookingDetail);
                }
                HttpContext.Session.SetInt32("_bookingid", booking.BookingId);
            }
        }

        public bool SavePayment()
        {
            int bookingId = (int)HttpContext.Session.GetInt32("_bookingid");
            if (bookingId != null)
            {
                BookingViewModel booking = GetSingleBookingRequest(bookingId);
                booking.IsPaid = true;
                booking.VerifyCode = Guid.NewGuid().ToString();
                UpdateBookingRequest(booking);
                IEnumerable<BookingDetailViewModel> bookingDetails = GetBookingDetailByIdRequest(bookingId);
                foreach (var position in bookingDetails)
                {
                    ScreeningPositionViewModel screeningPosition = GetScreeningPositionsDetailsRequest(position.PositionId);
                    if (!screeningPosition.IsBooked)
                    {
                        screeningPosition.IsBooked = true;
                        UpdateScreeningPositionsRequest(screeningPosition);
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }
            return bookingId != null;
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

        public ActionResult MomoPayment(int total)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán đặt vé tại rạp Cinemax";
            string returnUrl = $"https://localhost:7094/Booking/MomoResult";
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

        public ActionResult MomoResult()
        {
            //hiển thị thông báo cho người dùng
            string errorCode = HttpContext.Request.Query["errorCode"].ToString();
            if (errorCode == "0")
            {
                if (SavePayment())
                {
                    ViewBag.Message = "Thanh toán thành công qua ví điện tử Momo, cảm ơn bạn đã sử dụng dịch vụ đặt vé của CINEMAX";
                }
                else
                {
                    DeleteBooking();
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý, một trong số chỗ ngồi bạn chọn đã được đặt, thanh toán thất bại";
                }
            }
            else
            {
                DeleteBooking();
                ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn, thanh toán thất bại";
            }
            return View();
        }

        public ActionResult VnPayPayment(int total)
        {
            string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string returnUrl = "https://localhost:7094/Booking/VnPayResult";
            string tmnCode = "GHHNT2HB";
            string hashSecret = "BAGAOHAPRHKQZASKQZASVPRSAKPXNYXS";

            VnPayLib pay = new VnPayLib();

            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (total * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", "127.0.0.1"); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đặt vé xem phim tại Cinemax"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult VnPayResult()
        {
            if (HttpContext.Request.Query.Count() > 0)
            {
                string hashSecret = "BAGAOHAPRHKQZASKQZASVPRSAKPXNYXS"; //Chuỗi bí mật
                var vnpayData = HttpContext.Request.Query;
                VnPayLib pay = new VnPayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (var s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s.Key, s.Value);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = HttpContext.Request.Query["vnp_SecureHash"].ToString(); //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    string errorCode = HttpContext.Request.Query["vnp_ResponseCode"].ToString();
                    if (vnp_ResponseCode == "00")
                    {
                        if (SavePayment())
                        {
                            ViewBag.Message = "Thanh toán thành công qua VNPAY hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + ", cảm ơn bạn đã sử dụng dịch vụ đặt vé của CINEMAX";
                        }
                        else
                        {
                            DeleteBooking();
                            ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý, một trong số chỗ ngồi bạn chọn đã được đặt, thanh toán thất bại";
                        }
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        DeleteBooking();
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn VNPAY: " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    DeleteBooking();
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        //API Service
        public IEnumerable<BookingDetailViewModel> GetBookingDetailByIdRequest(int bookingId)
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