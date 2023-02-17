using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using X.PagedList;

namespace TheatreBookingSystem.AdminApp.Controllers
{
    public class TheatreController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/theatre");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public TheatreController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index(int id, int? page, string? key)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            HttpContext.Session.SetInt32("currentCinemaId", id);
            IEnumerable<TheatreViewModel> list = GetTheatreListRequest(id);
            if (!String.IsNullOrEmpty(key))
            {
                key = key.ToLower().Trim();
                list = list.Where(x => x.TheatreName.ToLower().Trim().Contains(key));
            }
            return View(list.Reverse().ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            TheatreViewModel theatre = GetTheatreDetailsRequest(id);
            return View(theatre);
        }

        public ActionResult Create()
        {
            IEnumerable<CinemaViewModel> cinemaList = GetCinemaListRequest();
            ViewBag.CinemaList = cinemaList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TheatreId,TheatreName,Capacity,CinemaId")] TheatreViewModel theatre)
        {
            if (theatre.Capacity % 10 != 0)
            {
                _notyf.Warning($"Số chỗ ngồi tối đa phải là bội số của 10", 3);
                return View(theatre);
            }
            HttpResponseMessage response = CreateTheatreRequest(theatre);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {theatre.TheatreName}", 3);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentCinemaId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(theatre);
        }

        public ActionResult Edit(int id)
        {
            TheatreViewModel theatre = GetTheatreDetailsRequest(id);
            IEnumerable<CinemaViewModel> cinemaList = GetCinemaListRequest();
            ViewBag.CinemaList = cinemaList;
            return View(theatre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TheatreViewModel theatre)
        {
            if (theatre.Capacity % 10 != 0)
            {
                _notyf.Warning($"Số chỗ ngồi tối đa phải là bội số của 10", 3);
                return View(theatre);
            }
            HttpResponseMessage response = UpdateTheatreRequest(theatre);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {theatre.TheatreName}", 3);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentCinemaId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(theatre);
        }

        public ActionResult Delete(int? id)
        {
            TheatreViewModel theatre = GetTheatreDetailsRequest(id);
            return View(theatre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteTheatreRequest(id);
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

        public IEnumerable<CinemaViewModel> GetCinemaListRequest()
        {
            IEnumerable<CinemaViewModel>? cinemas = null;
            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri("https://localhost:44322/api/cinema/getall");

            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            HttpResponseMessage response = _client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinemas = JsonConvert.DeserializeObject<IEnumerable<CinemaViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return cinemas;
        }

        //Response message
        public IEnumerable<TheatreViewModel> GetTheatreListRequest(int cinemaId)
        {
            IEnumerable<TheatreViewModel> theatres = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getallbycinema/{cinemaId}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                theatres = JsonConvert.DeserializeObject<IEnumerable<TheatreViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return theatres;
        }

        public TheatreViewModel GetTheatreDetailsRequest(int? id)
        {
            TheatreViewModel theatre = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                theatre = JsonConvert.DeserializeObject<TheatreViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                RedirectToAction("NotFound", "Home");
            }
            return theatre;
        }

        public HttpResponseMessage CreateTheatreRequest(TheatreViewModel theatre)
        {
            string data = JsonConvert.SerializeObject(theatre);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateTheatreRequest(TheatreViewModel theatre)
        {
            string data = JsonConvert.SerializeObject(theatre);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteTheatreRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }
    }
}