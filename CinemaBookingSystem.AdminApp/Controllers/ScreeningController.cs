using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using X.PagedList;

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

        public ActionResult Index(int? page, DateTime? key, string? Theatre)
        {
            IEnumerable<CinemaViewModel> cinemas = GetCinemaListRequest();
            ViewBag.Cinemas = new SelectList(cinemas, "CinemaId", "CinemaName");
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            IEnumerable<ScreeningViewModel> list = GetScreeningListRequest();
            if (!String.IsNullOrEmpty(key.ToString()))
            {
                list = list.Where(x => x.ShowTime.Date == key.Value.Date);
            }
            if (!String.IsNullOrEmpty(Theatre))
            {
                list = list.Where(x => x.TheatreId == Convert.ToInt32(Theatre));
            }
            return View(list.OrderByDescending(x => x.ShowTime).ToPagedList(pageNumber, pageSize));
        }

        public JsonResult GetTheatre(string id)
        {
            List<SelectListItem> theatres = new List<SelectListItem>();
            var theatresList = GetTheatreListRequest(Convert.ToInt32(id));
            return Json(theatresList.ToList());
        }

        public ActionResult Details(int id)
        {
            ScreeningViewModel screening = GetScreeningDetailsRequest(id);
            IEnumerable<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsDetailsRequest(id);
            screening.ScreeningPositions = screeningPositions;
            return View(screening);
        }

        public ActionResult ScreeningPositionsDetails(int id)
        {
            IEnumerable<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsDetailsRequest(id);
            return View(screeningPositions);
        }

        public ActionResult Create()
        {
            GetData();
            return View();
        }

        private void GetData()
        {
            IEnumerable<MovieViewModel> movieList = GetMovieListRequest();
            ViewBag.MovieList = movieList;
            IEnumerable<CinemaViewModel> cinemas = GetCinemaListRequest();
            ViewBag.Cinemas = new SelectList(cinemas, "CinemaId", "CinemaName");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ScreeningId,MovieId,ShowTime")] ScreeningViewModel screening, int positionPrice, string Theatre)
        {
            TheatreViewModel theatre = GetTheatreDetailsRequest(Convert.ToInt32(Theatre));
            if (screening.ShowTime < DateTime.Now.AddDays(1))
            {
                _notyf.Error("Thời gian chiếu không hợp lệ! Thời gian hợp lệ không được nhỏ hơn 1 ngày so với thời gian hiện tại", 5);
                GetData();
                return View(screening);
            }
            if (theatre != null)
            {
                screening.TheatreId = Convert.ToInt32(Theatre);
                screening.ShowStatus = false;
            }
            else
            {
                _notyf.Error("Phòng chiếu là thông tin bắt buộc", 5);
            }
            //Send API request
            HttpResponseMessage response = CreateScreeningRequest(screening);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công lịch chiếu", 3);
                string body = response.Content.ReadAsStringAsync().Result;
                screening = JsonConvert.DeserializeObject<ScreeningViewModel>(body);
                CreateScreeningPositions(theatre.Capacity, screening.ScreeningId, positionPrice);
                return RedirectToAction("Index", "Screening");
            }
            else
            {
                _notyf.Error("Không thể thêm vì trùng lịch chiếu", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            GetData();
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
        public IActionResult Edit(ScreeningViewModel screening, int? price)
        {
            //Send API request
            HttpResponseMessage response = UpdateScreeningRequest(screening);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Cập nhật thành công lịch chiếu", 3);
                if (price != null)
                {
                    UpdateScreeningPositions(screening.ScreeningId, (int)price);
                }
                return RedirectToAction("Details","Screening", new { id = screening.ScreeningId });
            }
            else
            {
                _notyf.Error("Thời gian chiếu điều chỉnh trùng với lịch chiếu khác", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            GetData();
            return View(screening);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        private void UpdateScreeningPositions(int screeningId, int price)
        {
            List<ScreeningPositionViewModel> screeningPositions = GetScreeningPositionsDetailsRequest(screeningId).Where(x => x.IsBooked == false).ToList();
            foreach (var item in screeningPositions)
            {
                item.Price = price;
                UpdateScreeningPositionsRequest(item);
            }
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
                return RedirectToAction("Index", "Screening");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Delete", "Screening", id);
            }
        }

        //Response message from API
        public IEnumerable<ScreeningViewModel> GetScreeningListRequest()
        {
            IEnumerable<ScreeningViewModel>? screenings = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
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
        public HttpResponseMessage UpdateScreeningPositionsRequest(ScreeningPositionViewModel screeningPosition)
        {
            string data = JsonConvert.SerializeObject(screeningPosition);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://localhost:44322/api/screeningposition/update");
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
        public ActionResult Calendar(string? Theatre, string from, string to)
        {
            if (!String.IsNullOrEmpty(Theatre))
            {
                var theatre = GetTheatreDetailsRequest(Convert.ToInt32(Theatre));
                ViewBag.Theatre = theatre.TheatreName;
                ViewBag.Cinema = theatre.Cinema.CinemaName;
                return View(new CalendarViewModel()
                {
                    TheatreId = theatre.TheatreId,
                    Start = from,
                    End = to,
                });
            }
            else
            {
                _notyf.Error("Hãy chọn phòng chiếu để xem lịch", 4);
            }
            return RedirectToAction("Index","Screening");
        }
        public JsonResult JsonScreeningCalendar(string start, string end, int? id)
        {
            DateTime d_startDate = new DateTime();
            DateTime d_endDate = new DateTime();
            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                if (DateTime.TryParseExact(start, "yyyy-MM-dd", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
                {
                    if (DateTime.TryParseExact(end, "yyyy-MM-dd", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                    {
                        d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    }
                }
            }
            else
            {
                d_startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                d_endDate = d_startDate.AddMonths(2).AddDays(-1);
            }
            var details = GetScreeningListRequest()
                .Where(x => x.ShowTime >= d_startDate
                && x.ShowTime <= d_endDate 
                && x.TheatreId == id)
                .AsEnumerable()
                .Select(x => new
                {
                    code = x.ScreeningId,
                    title = $"{x.Movie.MovieName}",
                    start = x.ShowTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    end = x.ShowTime.AddMinutes(x.Movie.RunningTime + 20).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    allDay = false,
                    className = x.ShowTime > DateTime.Now ? "ChuaChieu" : "DaChieu"
                });
            return Json(details);
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}