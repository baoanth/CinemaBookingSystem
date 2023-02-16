using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CinemaBookingSystem.WebApp.Controllers
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
            IEnumerable<MovieViewModel> list = GetMovieListRequest();
            foreach (var item in list)
            {
                IEnumerable<CommentViewModel> comments = GetCommentListRequest(item.MovieId);
                item.Comments = comments;
            }
            return View(list);
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("_clientname") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.Remove("_clientname");
                HttpContext.Session.Remove("_clientfullname");
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            return View();
        }

        private IEnumerable<CommentViewModel> GetCommentListRequest(object id)
        {
            IEnumerable<CommentViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"comment/getall/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<CommentViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin bình luận do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list.OrderBy(x => x.CommentedAt);
        }

        public IEnumerable<MovieViewModel> GetMovieListRequest()
        {
            IEnumerable<MovieViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "movie/getall");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}