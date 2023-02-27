using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using X.PagedList;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class CustomerContactController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/customercontact");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public CustomerContactController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<CustomerContactViewModel> list = null;
            HttpResponseMessage response = GetCustomerContactList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<CustomerContactViewModel>>(body);
                list = list.OrderByDescending(x => x.SendedAt);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            CustomerContactViewModel customerContact = null;
            HttpResponseMessage response = GetCustomerContactDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                customerContact = JsonConvert.DeserializeObject<CustomerContactViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(customerContact);
        }

        //Response message
        public HttpResponseMessage GetCustomerContactList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetRoleList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44322/api/role/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage GetCustomerContactDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage CreateCustomerContact(CustomerContactViewModel CustomerContact)
        {
            string data = JsonConvert.SerializeObject(CustomerContact);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44322/api/account/signup");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateCustomerContact(CustomerContactViewModel CustomerContact)
        {
            string data = JsonConvert.SerializeObject(CustomerContact);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteCustomerContact(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}