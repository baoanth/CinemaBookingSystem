using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class MovieController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public MovieController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies);
        }
        public IActionResult ComingSoon()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies.Where(x => x.ReleaseDate >= DateTime.UtcNow).ToList());
        }
        public IActionResult NowShowing()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies.Where(x => x.ReleaseDate <= DateTime.UtcNow).ToList());
        }
        public IActionResult TrailerWatch(int id)
        {
            MovieViewModel movie = GetMovieDetailsRequest(id);
            return View(movie);
        }
        public IActionResult Details(int id)
        {
            MovieViewModel movie = GetMovieDetailsRequest(id);
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            movies = movies.Where(x => x.MovieId != movie.MovieId);
            ViewBag.Movies = movies;
            return View(movie);
        }

        private MovieViewModel GetMovieDetailsRequest(int id)
        {
            MovieViewModel movie = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"movie/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<MovieViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return movie;
        }

        private IEnumerable<MovieViewModel> GetMovieListRequest()
        {
            IEnumerable<MovieViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "movie/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
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
    }
}
