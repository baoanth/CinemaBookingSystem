using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.Infrastructure.Core;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ApiControllerBase
    {
        private IUserService _userService;

        public LoginController(IErrorService errorService, IUserService userService) : base(errorService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("login")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] LoginViewModel loginViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    bool isValid = _userService.Login(loginViewModel.Username, loginViewModel.Password);
                    if (isValid) response = request.CreateResponse(HttpStatusCode.OK);
                    else response = request.CreateResponse(HttpStatusCode.NotFound);
                }
                return response;
            });
        }
    }
}