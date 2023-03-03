using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.AdminApp.Utils;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public ActionResult Index(int? page, string? fullname, DateTime? from, DateTime? to, string? status)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<CustomerContactViewModel> list = GetCustomerContactList();
            if (!String.IsNullOrEmpty(fullname))
            {
                fullname = fullname.ToLower().Trim();
                list = list.Where(x => x.CustomerName.ToLower().Trim().Contains(fullname));
            }
            if (from.HasValue && to.HasValue)
            {
                list = list.Where(x => x.SendedAt >= from && x.SendedAt <= to);
            }
            if (!String.IsNullOrEmpty(status))
            {
                if (status == "true")
                {
                    list = list.Where(x => x.Status == true);
                }
                else
                {
                    list = list.Where(x => x.Status == false);
                }
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            CustomerContactViewModel customerContact = GetCustomerContactDetails(id);
            return View(customerContact);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult SendMail(string sendto, string message, int id)
        {
            string response = MailUtil.Send(sendto, message);
            if (response == "OK")
            {
                var contact = GetCustomerContactDetails(id);
                contact.Status = true;
                UpdateCustomerContact(contact);
                _notyf.Success("Phản hồi đến khách hàng đã được gửi thông qua email", 4);
            }
            else
            {
                _notyf.Error("Gửi thất bại do lỗi xác thực", 4);
            }
            return RedirectToAction("Index", "CustomerContact");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteCustomerContact(id);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Xóa phản hồi thành công!", 4);
            }
            else
            {
                _notyf.Error("Không thể thực hiện được do lỗi API",4);
            }
            return RedirectToAction("Index", "CustomerContact");
        }

        //Response message
        public IEnumerable<CustomerContactViewModel> GetCustomerContactList()
        {
            IEnumerable<CustomerContactViewModel>? list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getall");
            request.Method = HttpMethod.Get;
            HttpResponseMessage response = _client.SendAsync(request).Result;
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
            return list;
        }

        public HttpResponseMessage GetRoleList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44322/api/role/getall");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
            
        }

        public CustomerContactViewModel GetCustomerContactDetails(int? id)
        {
            CustomerContactViewModel customerContact = new CustomerContactViewModel();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response =  _client.SendAsync(request).Result;
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
            return customerContact;
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