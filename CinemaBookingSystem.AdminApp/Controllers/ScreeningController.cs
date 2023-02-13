using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class ScreeningController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/screening");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public ScreeningController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult LandingPage()
        {
            IEnumerable<CinemaViewModel> cinemaList = GetCinemaListRequest();
            return View(cinemaList);
        }

        public ActionResult TheatreChoose(int id)
        {
            IEnumerable<TheatreViewModel> theatreList = GetTheatreListRequest(id);
            return View(theatreList);
        }

        public ActionResult Index(int id)
        {
            HttpContext.Session.SetInt32("currentTheatreId", id);
            IEnumerable<ScreeningViewModel> list = GetScreeningListRequest(id);
            return View(list);
        }

        public ActionResult Details(int id)
        {
            ScreeningViewModel screening = GetScreeningDetailsRequest(id);
            return View(screening);
        }

        public ActionResult ScreeningPositionsDetails(int id)
        {
            IEnumerable<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsDetailsRequest(id);
            return View(screeningPositions);
        }

        public ActionResult Create()
        {
            IEnumerable<MovieViewModel> movieList = GetMovieListRequest();
            ViewBag.MovieList = movieList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ScreeningId,MovieId,ShowTime")] ScreeningViewModel screening, int positionPrice)
        {
            if (screening.ShowTime < DateTime.Now.AddDays(1))
            {
                _notyf.Error("Thời gian chiếu không hợp lệ! Thời gian hợp lệ không được nhỏ hơn 1 ngày so với thời gian hiện tại", 5);
                return View(screening);
            }
            //Setting attribute for screening schedule
            int? currentTheatreId = HttpContext.Session.GetInt32("currentTheatreId");
            TheatreViewModel theatre = GetTheatreDetailsRequest(currentTheatreId);
            screening.ShowStatus = false;
            screening.TheatreId = theatre.TheatreId;

            //Send API request
            HttpResponseMessage response = CreateScreeningRequest(screening);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công lịch chiếu", 3);
                string body = response.Content.ReadAsStringAsync().Result;
                screening = JsonConvert.DeserializeObject<ScreeningViewModel>(body);
                CreateScreeningPositions(theatre.Capacity, screening.ScreeningId, positionPrice);
                return RedirectToAction("Index", new { id = currentTheatreId });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateScreeningPositions(int totalPositions, int screeningId, int price)
        {
            int row = totalPositions / 10;
            int col = 10;
            List<ScreeningPositionViewModel> screeningPositions = new List<ScreeningPositionViewModel>();
            for (int i = 1; i <= row; i++)
            {
                for (int j = 1; j <= col; j++)
                {
                    ScreeningPositionViewModel screeningPosition = new ScreeningPositionViewModel()
                    {
                        IsBooked = false,
                        ScreeningId = screeningId,
                        Price = price,
                        Row = ((char)('A' + (i - 1) % 26)).ToString(),
                        Column = j.ToString(),
                    };
                    screeningPositions.Add(screeningPosition);
                }
            }
            HttpResponseMessage response = CreateScreeningPositionsRequest(screeningPositions);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Đã thêm chỗ ngồi cho lịch chiếu", 3);
            }
            else
            {
                _notyf.Success($"Không thể thêm chỗ ngồi do lỗi server", 3);
            }
        }

        public ActionResult Edit(int id)
        {
            ScreeningViewModel screening = GetScreeningDetailsRequest(id);
            IEnumerable<MovieViewModel> movieList = GetMovieListRequest();
            ViewBag.MovieList = movieList;
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ScreeningViewModel screening)
        {
            //Send API request
            HttpResponseMessage response = UpdateScreeningRequest(screening);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Cập nhật thành công lịch chiếu", 3);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentTheatreId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(screening);
        }

        public ActionResult Delete(int? id)
        {
            ScreeningViewModel screening = GetScreeningDetailsRequest(id);
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _notyf.Warning($"Đang tiến hành xóa các chỗ ngồi của lịch chiếu!", 4);
            if (DeleteScreeningPositionsRequest(id).IsSuccessStatusCode)
            {
                _notyf.Success($"Đã xóa tất cả chỗ ngồi thuộc lịch chiếu!", 4);
            }
            else
            {
                _notyf.Error("Không thể xóa chỗ ngồi do lỗi server", 4);
            }
            HttpResponseMessage response = DeleteScreeningRequest(id);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Xóa thành công khỏi danh sách!", 4);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentTheatreId") });
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("currentTheatreId") });
            }
        }

        //Response message from API
        public IEnumerable<ScreeningViewModel> GetScreeningListRequest(int id)
        {
            IEnumerable<ScreeningViewModel>? screenings = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getallbytheatre/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                screenings = JsonConvert.DeserializeObject<IEnumerable<ScreeningViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return screenings;
        }

        public ScreeningViewModel GetScreeningDetailsRequest(int? id)
        {
            ScreeningViewModel? screening = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                screening = JsonConvert.DeserializeObject<ScreeningViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return screening;
        }

        public HttpResponseMessage CreateScreeningRequest(ScreeningViewModel screening)
        {
            string data = JsonConvert.SerializeObject(screening);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateScreeningRequest(ScreeningViewModel screening)
        {
            string data = JsonConvert.SerializeObject(screening);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteScreeningRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
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

        public IEnumerable<TheatreViewModel> GetTheatreListRequest(int id)
        {
            IEnumerable<TheatreViewModel>? theatres = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/theatre/getallbycinema/{id}");
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
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return theatres;
        }

        public TheatreViewModel GetTheatreDetailsRequest(int? theatreId)
        {
            TheatreViewModel? theatre = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/theatre/getsingle/{theatreId}");
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
                _notyf.Error("Không thể tìm thấy thông tin phòng chiếu từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return theatre;
        }

        public IEnumerable<MovieViewModel> GetMovieListRequest()
        {
            IEnumerable<MovieViewModel>? movies = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44322/api/movie/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                movies = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return movies;
        }

        public HttpResponseMessage CreateScreeningPositionsRequest(IEnumerable<ScreeningPositionViewModel> screeningPositions)
        {
            string data = JsonConvert.SerializeObject(screeningPositions);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/screeningposition/createmulti");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteScreeningPositionsRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/screeningposition/deletebyscreening/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }

        public IEnumerable<ScreeningPositionViewModel> GetScreeningPositionsDetailsRequest(int? id)
        {
            IEnumerable<ScreeningPositionViewModel>? screeningPositions = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/screeningposition/getallbyscreening/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
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
    }
}