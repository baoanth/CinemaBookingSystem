using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper, IErrorService errorService)
        {
            _userService = userService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpPost]
        [Route("systemlogin")]
        public ActionResult SystemLogin([FromHeader, Required] string CBSToken, [FromBody] LoginViewModel login)
        {
            const int ADMIN_ROLE = 2;
            bool IsValid = _userService.Login(login.Username, login.Password);
            if (!IsValid) return BadRequest();
            else
            {
                var user = _userService.GetByUsername(login.Username);
                if (user.RoleID != ADMIN_ROLE)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(login);
                }
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromHeader, Required] string CBSToken, [FromBody] LoginViewModel login)
        {
            bool IsValid = _userService.Login(login.Username, login.Password);
            if (!IsValid) return BadRequest();
            else
            {
                return Ok(login);
            }
        }

        [HttpPost]
        [Route("signin")]
        public ActionResult Signin([FromHeader, Required] string CBSToken, [FromBody] SignupViewModel signup)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var user = _mapper.Map<User>(signup);
                    _userService.Signup(user);
                    _userService.SaveChanges();
                    return Ok("Sign-in successful!");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error \"{ve.ErrorMessage}\"");
                        }
                    }
                    _errorService.LogError(ex);
                    return BadRequest(ex.InnerException.Message);
                }
                catch (DbUpdateException dbEx)
                {
                    _errorService.LogError(dbEx);
                    return BadRequest(dbEx.InnerException.Message);
                }
                catch (Exception ex)
                {
                    _errorService.LogError(ex);
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}