using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class StatisticController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public StatisticController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            IEnumerable<BookingDetailViewModel> bookingDetails = GetBookingDetailListRequest();
            var monthTopMovie = bookingDetails.Where(x => x.Booking.BookedAt.Month == DateTime.Now.Month).GroupBy(x => new { x.ScreeningPosition.Screening.Movie.MovieId, x.ScreeningPosition.Screening.Movie.MovieName }, (Key, group) => new TopMovieViewModel()
            {
                MovieId = Key.MovieId,
                MovieName = Key.MovieName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).ToList();
            var yearTopMovie = bookingDetails.Where(x => x.Booking.BookedAt.Year == DateTime.Now.Year).GroupBy(x => new { x.ScreeningPosition.Screening.Movie.MovieId, x.ScreeningPosition.Screening.Movie.MovieName }, (Key, group) => new TopMovieViewModel()
            {
                MovieId = Key.MovieId,
                MovieName = Key.MovieName,
                TotalPrice = group.Sum(x => x.ScreeningPosition.Price),
                TotalSell = group.Count(),
            }).ToList();
            ViewBag.MonthTopMovie = monthTopMovie;
            ViewBag.YearTopMovie = yearTopMovie;

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
    }
}
