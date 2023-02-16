using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using X.PagedList;

namespace CinemaBookingSystem.AdminApp.Controllers
{
    public class StaffController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/user");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public StaffController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            
            IEnumerable<UserViewModel> list = null;
            HttpResponseMessage response = GetUserList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(list.Reverse().ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Details(int? id)
        {
            UserViewModel user = null;
            HttpResponseMessage response = GetUserDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                
            }
            return View(user);
        }

        public ActionResult Create()
        {
            IEnumerable<RoleViewModel> roleList = null;
            HttpResponseMessage response = GetRoleList();
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                roleList = JsonConvert.DeserializeObject<IEnumerable<RoleViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,Username,Password,FullName,DOB,Email,PhoneNumber,Address,RoleId")] UserViewModel user)
        {
            HttpResponseMessage response = CreateUser(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Thêm mới thành công: {user.Username}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server hoặc thông tin chưa hợp lệ", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            UserViewModel user = null;
            HttpResponseMessage response = GetUserDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                
            }

            IEnumerable<RoleViewModel> roleList = null;
            HttpResponseMessage roleResponse = GetRoleList();
            if (roleResponse.IsSuccessStatusCode)
            {
                string body = roleResponse.Content.ReadAsStringAsync().Result;
                roleList = JsonConvert.DeserializeObject<IEnumerable<RoleViewModel>>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin rạp từ server");
                _notyf.Error($"Status code: {(int)roleResponse.StatusCode}, Message: {roleResponse.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)roleResponse.StatusCode, roleResponse.ReasonPhrase);
            }
            ViewBag.RoleList = roleList;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel user)
        {
            HttpResponseMessage response = UpdateUser(user);
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Chỉnh sửa thành công: {user.Username}", 3);
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Không thể thực hiện do lỗi server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            UserViewModel user = null;
            HttpResponseMessage response = GetUserDetails(id);
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể tìm thấy thông tin từ server", 4);
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}", 4);
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = DeleteUser(id);
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
        public HttpResponseMessage GetUserList()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getallstaff");
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

        public HttpResponseMessage GetUserDetails(int? id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/getsingle/{id}");
            request.Method = HttpMethod.Get;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage CreateUser(UserViewModel User)
        {
            string data = JsonConvert.SerializeObject(User);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://localhost:44322/api/account/signup");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage UpdateUser(UserViewModel User)
        {
            string data = JsonConvert.SerializeObject(User);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "/update");
            request.Method = HttpMethod.Post;

            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        public HttpResponseMessage DeleteUser(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"/delete/{id}");
            request.Method = HttpMethod.Delete;

            return _client.SendAsync(request).Result;
        }
    }
}