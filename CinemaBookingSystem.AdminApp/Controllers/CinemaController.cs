using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class CinemaController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        public const string SessionKeyName = "_name";

        public CinemaController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
        }

        [Route("/cinema")]
        public ActionResult Index()
        {
            IEnumerable<CinemaViewModel> list = null;
            HttpResponseMessage response = GetCinemaList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<CinemaViewModel>>(body);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(list);
        }

        [Route("/cinema/details/{id}")]
        public IActionResult Details(int? id)
        {
            CinemaViewModel cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(cinema);
        }

        [Route("/cinema/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CinemaId,CinemaName,FAX,Hotline,Address,City,Region")] CinemaViewModel cinema)
        {
            if (CreateCinema(cinema).IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin không hợp lệ!");
            }
            return View();
        } 

        [Route("/cinema/edit/{id}")]
        public ActionResult Edit(int id)
        {
            CinemaViewModel cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(cinema);
        }

        // POST: CinemaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CinemaViewModel cinema)
        {
            if (UpdateCinema(cinema).IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin không hợp lệ!");
            }
            return View();
        }
        [Route("/cinema/delete/{id}")]
        public ActionResult Delete(int id)
        {
            CinemaViewModel cinema = null;
            HttpResponseMessage response = GetCinemaDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                cinema = JsonConvert.DeserializeObject<CinemaViewModel>(body);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return View("../Shared/NotFound");
            }
            return View(cinema);
        }

        // POST: CinemaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            return View();
        }
        public HttpResponseMessage GetCinemaList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/cinema/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage GetCinemaDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/cinema/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage CreateCinema(CinemaViewModel cinema)
        {
            string data = JsonConvert.SerializeObject(cinema);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/location/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage UpdateCinema(CinemaViewModel cinema)
        {
            string data = JsonConvert.SerializeObject(cinema);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/cinema/update");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }
        public HttpResponseMessage GetLocationDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/location/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            return _client.SendAsync(request).Result;
        }
    }
}