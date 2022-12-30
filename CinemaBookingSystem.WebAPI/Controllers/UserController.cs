using AutoMapper;
using CinemaBookingSystem.Model.Models;
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
            if (listUser.Count() <= 0)
            {
                return NotFound("There is no user!");
            }
            var listUserVm = _mapper.Map<IEnumerable<UserViewModel>>(listUser);
            return Ok(listUserVm);
        }
        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Model state is invalid!");
            else
            {
                var user = _mapper.Map<User>(userViewModel);
                _userService.Add(user);
                _userService.SaveChanges();
                return Ok(user);
            }
        }
        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] UserViewModel userVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            else
            {
                var user = _mapper.Map<User>(userVm);
                _userService.Update(user);
                _userService.SaveChanges();
                return Ok(userVm);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var user = _userService.GetById(id);
            bool IsValid = user != null;
            if (!IsValid) return BadRequest("The Id is not exist!");
            else
            {
                _userService.Delete(id);
                _userService.SaveChanges();
                return Ok("Deleted");
            }
        }
    }
}
