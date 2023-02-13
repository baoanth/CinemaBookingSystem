using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using CinemaBookingSystem.WebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CinemaBookingSystem.WebApp.Controllers
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
            return View(pickedSeats);
        }

        public ActionResult Checkout(int paymentid, int total)
        {
            switch (paymentid)
            {
                case 4:
                    return MomoPayment(total);
                default:
                    return MomoPayment(total);
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
            string orderInfo = "Thanh toán đặt vé Cinemax";
            string returnUrl = "https://localhost:7094/Home/ConfirmPaymentClient";
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

        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }

        #endregion MOMO PAYMENT

        //private ScreeningViewModel GetScreeningDetailsRequest(int id)
        //{
        //    ScreeningViewModel screening = null;
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    request.RequestUri = new Uri(_baseUrl + $"screening/getsingle/{id}");
        //    request.Method = HttpMethod.Get;

        //    HttpResponseMessage response = _client.SendAsync(request).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string body = response.Content.ReadAsStringAsync().Result;
        //        screening = JsonConvert.DeserializeObject<ScreeningViewModel>(body);
        //    }
        //    else
        //    {
        //        _notyf.Error("Không thể lấy thông tin do lỗi server");
        //        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }
        //    return screening;
        //}

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