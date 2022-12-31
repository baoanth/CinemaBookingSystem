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
        [Route("login")]
        public ActionResult Login([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] LoginViewModel login)
        {
            bool IsValid = _userService.Login(login.Username, login.Password);
            if (!IsValid) return BadRequest("Username or Password is incorrect!");
            else
            {
                return Ok("Account verified!");
            }
        }

        [HttpPost]
        [Route("signin")]
        public ActionResult Signin([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var user = _mapper.Map<User>(userViewModel);
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