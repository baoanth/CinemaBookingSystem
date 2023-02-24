using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class CinemaController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/cinema");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public CinemaController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public IActionResult Index(int? page, string? key, string? region)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            IEnumerable<CinemaViewModel> list = GetCinemaList();
            if (!String.IsNullOrEmpty(key))
            {
                key = key.ToLower().Trim();
                list = list.Where(mv => mv.CinemaName.ToLower().Trim().Contains(key));
            }
            if (!String.IsNullOrEmpty(region))
            {
                list = list.Where(mv => mv.Region == region);
            }
            return View(list.OrderBy(x => x.City).ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            CinemaViewModel? cinema = GetCinemaDetails(id);

            return View(cinema);
        }

        public IEnumerable<CinemaViewModel> GetCinemaList()
        {
            IEnumerable<CinemaViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/getall");
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

        public CinemaViewModel GetCinemaDetails(int? id)
        {
            CinemaViewModel? cinema = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                RedirectToAction("NotFound", "Home");
            }
            return cinema;
        }
    }
}