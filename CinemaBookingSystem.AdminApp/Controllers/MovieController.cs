using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class MovieController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/movie");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(INotyfService notyf, IWebHostEnvironment webHostEnvironment)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Index()
        {
            IEnumerable<MovieViewModel>? list = GetMovieList();
            return View(list.Reverse());
        }

        public IActionResult Details(int? id)
        {
            MovieViewModel? movie = GetMovieDetails(id);
            return View(movie);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormFile postedThumpnail, IFormFile postedBanner, [Bind("MovieId,MovieName,Director,Cast,ReleaseDate,Genres,RunningTime,Rated,TrailerURL,ThumpnailImg,BannerImg,Description")] MovieViewModel movie)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedThumpnail != null && postedThumpnail.Length > 0)
            {
                string fileName = Path.GetFileName(postedThumpnail.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                movie.ThumpnailImg = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "thumpnails", fileName), FileMode.Create))
                {
                    postedThumpnail.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {movie.ThumpnailImg}", 3);
                }
            }
            if (postedBanner != null && postedBanner.Length > 0)
            {
                string fileName = Path.GetFileName(postedBanner.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                movie.BannerImg = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "banners", fileName), FileMode.Create))
                {
                    postedBanner.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {movie.BannerImg}", 3);
                }
            }
            HttpResponseMessage response = CreateMovie(movie);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới phim thành công: {movie.MovieName}", 3);
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
            MovieViewModel? movie = GetMovieDetails(id);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieViewModel movie, IFormFile postedThumpnail, IFormFile postedBanner)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedThumpnail != null && postedThumpnail.Length > 0)
            {
                if (!String.IsNullOrEmpty(movie.ThumpnailImg))
                {
                    var oldFilePath = Path.Combine(path, "thumpnails", movie.ThumpnailImg);
                    if (Directory.Exists(oldFilePath))
                    {
                        Directory.Delete(oldFilePath);
                    }
                }
                string fileName = Path.GetFileName(postedThumpnail.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                movie.ThumpnailImg = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "thumpnails", fileName), FileMode.Create))
                {
                    postedThumpnail.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {movie.ThumpnailImg}", 3);
                }
            }
            if (postedBanner != null && postedBanner.Length > 0)
            {
                if (!String.IsNullOrEmpty(movie.BannerImg))
                {
                    var oldFilePath = Path.Combine(path, "banners", movie.BannerImg);
                    if (Directory.Exists(oldFilePath))
                    {
                        Directory.Delete(oldFilePath);
                    }
                }
                string fileName = Path.GetFileName(postedBanner.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                movie.BannerImg = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "banners", fileName), FileMode.Create))
                {
                    postedBanner.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {movie.BannerImg}", 3);
                }
            }
            HttpResponseMessage response = UpdateMovie(movie);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {movie.MovieName}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(movie);
        }

        public ActionResult Delete(int? id)
        {
            MovieViewModel? movie = GetMovieDetails(id);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteMovie(id);
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
        public IEnumerable<MovieViewModel> GetMovieList()
        {
            IEnumerable<MovieViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/getall");
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

        public MovieViewModel GetMovieDetails(int? id)
        {
            MovieViewModel movie = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<MovieViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                RedirectToAction("NotFound", "Home");
            }
            return movie;
        }

        public HttpResponseMessage CreateMovie(MovieViewModel movie)
        {
            string data = JsonConvert.SerializeObject(movie);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateMovie(MovieViewModel movie)
        {
            string data = JsonConvert.SerializeObject(movie);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteMovie(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}