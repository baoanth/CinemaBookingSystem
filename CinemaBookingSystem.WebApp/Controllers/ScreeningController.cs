using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class ScreeningController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
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

        public IActionResult CinemaChoose(int id)
        {
            MovieViewModel movie = GetMovieDetailsRequest(id);
            IEnumerable<CinemaViewModel> list = GetCinemaList();
            ViewBag.Cinemas = list;
            return View(movie);
        }

        public IActionResult ScreeningChoose(int movieId, int cinemaId)
        {
            IEnumerable<ScreeningViewModel> screenings = GetSreeningRequest(movieId, cinemaId);
            return View(screenings);
        }

        private IEnumerable<ScreeningViewModel> GetSreeningRequest(int movieId, int cinemaId)
        {
            IEnumerable<ScreeningViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"screening/getallbycinemaandmovie/{cinemaId}/{movieId}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ScreeningViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }

        private MovieViewModel GetMovieDetailsRequest(int id)
        {
            MovieViewModel movie = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"movie/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<MovieViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return movie;
        }

        public IEnumerable<CinemaViewModel> GetCinemaList()
        {
            IEnumerable<CinemaViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "cinema/getall");
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
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }
    }
}