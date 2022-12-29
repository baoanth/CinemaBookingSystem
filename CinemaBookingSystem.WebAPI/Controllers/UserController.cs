using AutoMapper;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken)
        {
            var listUser = _userService.GetAll();
            var listUserVm = _mapper.Map<IEnumerable<UserViewModel>>(listUser);
            return Ok(listUserVm);
        }
        [HttpGet]
        [Route("getbyrole")]
        public ActionResult GetByRole([FromHeader, Required] string CinemaBookingSystemToken, int roleId)
        {
            var listUser = _userService.GetByRole(roleId);
            var listUserVm = _mapper.Map<IEnumerable<UserViewModel>>(listUser);
            return Ok(listUserVm);
        }
        [HttpGet]
        [Route("getbyusername")]
        public ActionResult GetByUsername([FromHeader, Required] string CinemaBookingSystemToken, string username)
        {
            var user = _userService.GetByUsername(username);
            var userVm = _mapper.Map<UserViewModel>(user);
            return Ok(userVm);
        }
        [HttpGet]
        [Route("search")]
        public ActionResult Search([FromHeader, Required] string CinemaBookingSystemToken, string keywords)
        {
            var listUser = _userService.Search(keywords);
            var listUserVm = _mapper.Map<UserViewModel>(listUser);
            return Ok(listUserVm);
        }
    }
}
