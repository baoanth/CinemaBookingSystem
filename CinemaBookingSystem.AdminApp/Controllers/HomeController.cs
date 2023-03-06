using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public HomeController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            IEnumerable<BookingDetailViewModel> bookingDetails = GetBookingDetailListRequest();
            var monthTopMovie = bookingDetails.Where(x => x.Booking.BookedAt.Month == DateTime.Now.Month && x.Booking.IsPaid).GroupBy(x => new { x.ScreeningPosition.Screening.Movie.MovieId, x.ScreeningPosition.Screening.Movie.MovieName }, (Key, group) => new TopMovieViewModel()
            {
                MovieId = Key.MovieId,
                MovieName = Key.MovieName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).OrderByDescending(x => x.TotalPrice).ToList();
            var yearTopMovie = bookingDetails.Where(x => x.Booking.BookedAt.Year == DateTime.Now.Year && x.Booking.IsPaid).GroupBy(x => new { x.ScreeningPosition.Screening.Movie.MovieId, x.ScreeningPosition.Screening.Movie.MovieName }, (Key, group) => new TopMovieViewModel()
            {
                MovieId = Key.MovieId,
                MovieName = Key.MovieName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).OrderByDescending(x => x.TotalPrice).ToList();
            var todayTopMovie = bookingDetails.Where(x => x.Booking.BookedAt.Date == DateTime.Now.Date && x.Booking.IsPaid).GroupBy(x => new { x.ScreeningPosition.Screening.Movie.MovieId, x.ScreeningPosition.Screening.Movie.MovieName }, (Key, group) => new TopMovieViewModel()
            {
                MovieId = Key.MovieId,
                MovieName = Key.MovieName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).OrderByDescending(x => x.TotalPrice).ToList();
            ViewBag.MonthTopMovie = monthTopMovie;
            ViewBag.YearTopMovie = yearTopMovie;
            ViewBag.TodayTopMovie = todayTopMovie;

            var topCustomer = bookingDetails.Where(x => x.Booking.IsPaid).GroupBy(x => new { x.Booking.User.UserId, x.Booking.User.FullName }, (Key, group) => new TopCustomerViewModel()
            {
                UserId = Key.UserId,
                Fullname = Key.FullName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).OrderByDescending(x => x.TotalPrice).Take(5).ToList();
            ViewBag.TopCustomer = topCustomer;

            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            foreach (var item in movies)
            {
                IEnumerable<ScreeningViewModel> screenings = GetScreeningListRequest(item.MovieId);
                item.Screenings = screenings.Where(x => x.ShowTime > DateTime.Now);
            }
            ViewBag.AllMovies = movies.Count();
            ViewBag.NowShowingMovies = movies.Where(x => x.ReleaseDate < DateTime.Now && x.Screenings.Count() != 0).Count();
            ViewBag.ComingSoonMovies = movies.Where(x => x.ReleaseDate > DateTime.Now).Count();

            IEnumerable<CinemaViewModel> cinemas = GetCinemaListRequest();
            ViewBag.Cinemas = cinemas.Count();

            IEnumerable<UserViewModel> users = GetUserListRequest();
            ViewBag.Users = users.Where(x => x.RoleId == 3).Count();
            ViewBag.Administrators = users.Where(x => x.RoleId == 1).Count();

            return View(bookingDetails);
        }

        private IEnumerable<BookingDetailViewModel> GetBookingDetailListRequest()
        {
            IEnumerable<BookingDetailViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"bookingdetail/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<BookingDetailViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }

        private IEnumerable<MovieViewModel> GetMovieListRequest()
        {
            IEnumerable<MovieViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"movie/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }

        private IEnumerable<CinemaViewModel> GetCinemaListRequest()
        {
            IEnumerable<CinemaViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"cinema/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
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
            return list;
        }

        private IEnumerable<UserViewModel> GetUserListRequest()
        {
            IEnumerable<UserViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"user/getall");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }

        private IEnumerable<ScreeningViewModel> GetScreeningListRequest(int id)
        {
            IEnumerable<ScreeningViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"screening/getallbymovie/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ScreeningViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin bình luận do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        } 
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("_name") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                HttpContext.Session.Remove("_name");
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}