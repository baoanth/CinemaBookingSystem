using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class SlideController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/slide");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlideController(INotyfService notyf, IWebHostEnvironment webHostEnvironment)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Index()
        {
            IEnumerable<SlideViewModel> list = GetSlideList();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SlideId,Name,Image,Description")] SlideViewModel slide, IFormFile postedFile)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images\\slides");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedFile != null && postedFile.Length > 0)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                slide.Image = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {slide.Image}", 3);
                }
            }
            HttpResponseMessage response = Createslide(slide);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {slide.Name}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(slide);
        }

        public ActionResult Edit(int id)
        {
            SlideViewModel slide = GetSlideDetails(id);
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SlideViewModel slide, IFormFile postedFile)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images\\slides");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedFile != null && postedFile.Length > 0)
            {
                var oldFilePath = Path.Combine(path, slide.Image);
                if (Directory.Exists(oldFilePath))
                {
                    Directory.Delete(oldFilePath);
                }
                string fileName = Path.GetFileName(postedFile.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                slide.Image = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {slide.Image}", 3);
                }
            }
            HttpResponseMessage response = UpdateSlideRequest(slide);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {slide.Name}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(slide);
        }

        public ActionResult Delete(int? id)
        {
            SlideViewModel slide = GetSlideDetails(id);
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = Deleteslide(id);
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
        public IEnumerable<SlideViewModel> GetSlideList()
        {
            IEnumerable<SlideViewModel> slides = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                slides = JsonConvert.DeserializeObject<IEnumerable<SlideViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                RedirectToAction("NotFound", "Home");
            }
            return slides;
        }
        public SlideViewModel GetSlideDetails(int? id)
        {
            SlideViewModel slide = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                slide = JsonConvert.DeserializeObject<SlideViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                RedirectToAction("NotFound", "Home");
            }
            return slide;
        }
        public HttpResponseMessage Createslide(SlideViewModel slide)
        {
            string data = JsonConvert.SerializeObject(slide);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage UpdateSlideRequest(SlideViewModel slide)
        {
            string data = JsonConvert.SerializeObject(slide);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage Deleteslide(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }
    }
}
