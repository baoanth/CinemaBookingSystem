using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class CinemaController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/cinema");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public CinemaController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index()
        {
            IEnumerable<CinemaViewModel>? list = null;
            HttpResponseMessage response = GetCinemaList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<CinemaViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(list);
        }

        public IActionResult Details(int? id)
        {
            CinemaViewModel? cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(cinema);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CinemaId,CinemaName,FAX,Hotline,Address,City,Region")] CinemaViewModel cinema)
        {
            HttpResponseMessage response = CreateCinema(cinema);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {cinema.CinemaName}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            CinemaViewModel? cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CinemaViewModel cinema)
        {
            HttpResponseMessage response = UpdateCinema(cinema);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {cinema.CinemaName}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(cinema);
        }

        public ActionResult Delete(int? id)
        {
            CinemaViewModel cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("NotFound", "Home");
            }
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteCinema(id);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Xóa thành công khỏi danh sách!", 4);
                return RedirectToAction("Index");
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
        public HttpResponseMessage GetCinemaList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetCinemaDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage CreateCinema(CinemaViewModel cinema)
        {
            string data = JsonConvert.SerializeObject(cinema);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateCinema(CinemaViewModel cinema)
        {
            string data = JsonConvert.SerializeObject(cinema);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteCinema(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}