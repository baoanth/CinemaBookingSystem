using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using X.PagedList;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class ArticleController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/article");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public ArticleController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index(int? page, string? key)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            IEnumerable<ArticleViewModel>? list = null;
            HttpResponseMessage response = GetArticleListRequest();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ArticleViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            if (!String.IsNullOrEmpty(key))
            {
                key = key.ToLower().Trim();
                list = list.Where(x => x.ArticleTitle.ToLower().Trim().Contains(key));
            }
            return View(list.Reverse().ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            ArticleViewModel? article = null;
            HttpResponseMessage response = GetArticleDetailsRequest(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                article = JsonConvert.DeserializeObject<ArticleViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(article);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ArticleId,ArticleTitle,ArticleContent")] ArticleViewModel article, IFormFile postedImage, IFormFile postedVideo)
        {
            article.CreatedDate = DateTime.Now;
            article.CreatedBy = (int)HttpContext.Session.GetInt32("_id");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images\\articles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedImage != null && postedImage.Length > 0)
            {
                string fileName = Path.GetFileName(postedImage.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                article.ArticleImage = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedImage.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {article.ArticleImage}", 3);
                }
            }
            if (postedVideo != null && postedVideo.Length > 0)
            {
                string fileName = Path.GetFileName(postedVideo.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                article.ArticleVideo = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "vids", fileName), FileMode.Create))
                {
                    postedVideo.CopyTo(stream);
                    _notyf.Success($"Upload video thành công: {article.ArticleVideo}", 3);
                }
            }
            HttpResponseMessage response = CreateArticleRequest(article);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {article.ArticleTitle}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);

                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ArticleViewModel? article = null;
            HttpResponseMessage response = GetArticleDetailsRequest(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                article = JsonConvert.DeserializeObject<ArticleViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);

                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel article, IFormFile postedImage, IFormFile postedVideo)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\sharedmedia\\images\\articles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (postedImage != null && postedImage.Length > 0)
            {
                if (!String.IsNullOrEmpty(article.ArticleImage))
                {
                    var oldFilePath = Path.Combine(path, article.ArticleImage);
                    if (Directory.Exists(oldFilePath))
                    {
                        Directory.Delete(oldFilePath);
                    }
                }
                string fileName = Path.GetFileName(postedImage.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                article.ArticleImage = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedImage.CopyTo(stream);
                    _notyf.Success($"Upload ảnh thành công: {article.ArticleImage}", 3);
                }
            }
            if (postedVideo != null && postedVideo.Length > 0)
            {
                if (!String.IsNullOrEmpty(article.ArticleVideo))
                {
                    var oldFilePath = Path.Combine(path, "vids", article.ArticleVideo);
                    if (Directory.Exists(oldFilePath))
                    {
                        Directory.Delete(oldFilePath);
                    }
                }
                string fileName = Path.GetFileName(postedVideo.FileName);
                fileName = Guid.NewGuid().ToString() + fileName;
                article.ArticleVideo = fileName;
                using (FileStream stream = new FileStream(Path.Combine(path, "vids", fileName), FileMode.Create))
                {
                    postedVideo.CopyTo(stream);
                    _notyf.Success($"Upload video thành công: {article.ArticleVideo}", 3);
                }
            }
            HttpResponseMessage response = UpdateArticleRequest(article);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {article.ArticleTitle}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);

                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(article);
        }

        public ActionResult Delete(int? id)
        {
            ArticleViewModel article = null;
            HttpResponseMessage response = GetArticleDetailsRequest(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                article = JsonConvert.DeserializeObject<ArticleViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);

                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteArticleRequest(id);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Xóa thành công khỏi danh sách!", 4);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);

                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return RedirectToAction("Index");
            }
        }

        //Response message
        public HttpResponseMessage GetArticleListRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetArticleDetailsRequest(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage CreateArticleRequest(ArticleViewModel article)
        {
            string data = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/create");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateArticleRequest(ArticleViewModel article)
        {
            string data = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteArticleRequest(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}