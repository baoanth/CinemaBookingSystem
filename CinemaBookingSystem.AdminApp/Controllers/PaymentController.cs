﻿using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class PaymentController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/payment");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public PaymentController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index()
        {
            IEnumerable<PaymentViewModel> list = null;
            HttpResponseMessage response = GetPaymentList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<PaymentViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PaymentId,PaymentMethod")] PaymentViewModel payment)
        {
            HttpResponseMessage response = CreatePayment(payment);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {payment.PaymentMethod}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(payment);
        }

        public ActionResult Edit(int id)
        {
            PaymentViewModel payment = null;
            HttpResponseMessage response = GetPaymentDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                payment = JsonConvert.DeserializeObject<PaymentViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentViewModel payment)
        {
            HttpResponseMessage response = UpdatePayment(payment);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {payment.PaymentMethod}", 3);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentCinemaId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(payment);
        }

        public ActionResult Delete(int? id)
        {
            PaymentViewModel payment = null;
            HttpResponseMessage response = GetPaymentDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                payment = JsonConvert.DeserializeObject<PaymentViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeletePayment(id);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Xóa thành công khỏi danh sách!", 4);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentCinemaId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Index");
            }
        }
        //Response message
        public HttpResponseMessage GetPaymentList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage GetPaymentDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage CreatePayment(PaymentViewModel payment)
        {
            string data = JsonConvert.SerializeObject(payment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage UpdatePayment(PaymentViewModel payment)
        {
            string data = JsonConvert.SerializeObject(payment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage DeletePayment(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }
    }
}
