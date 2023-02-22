using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IErrorService errorService)
        {
            _userService = userService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listUser = _userService.GetAll();
            var listUserVm = _mapper.Map<IEnumerable<UserViewModel>>(listUser);
            return Ok(listUserVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return BadRequest("There is no user!");
            else
            {
                var userVm = _mapper.Map<UserViewModel>(user);
                return Ok(user);
            }
        }

        [HttpGet]
        [Route("getbyrole")]
        public ActionResult GetByRole([FromHeader, Required] string CBSToken, int roleId)
        {
            var listUser = _userService.GetByRole(roleId);
            if (listUser.Count() <= 0) return BadRequest("There is no user!");
            else
            {
                var listUserVm = _mapper.Map<IEnumerable<UserViewModel>>(listUser);
                return Ok(listUserVm);
            }
        }

        [HttpGet]
        [Route("getbyusername")]
        public ActionResult GetByUsername([FromHeader, Required] string CBSToken, string username)
        {
            var user = _userService.GetByUsername(username);
            if (user == null) return BadRequest("The input Username doesn't exist");
            else
            {
                var userVm = _mapper.Map<UserViewModel>(user);
                return Ok(userVm);
            }
        }

        [HttpPost]
        [Route("changepassword")]
        public ActionResult ChangePassword([FromHeader, Required] string CBSToken, ChangePasswordViewModel changes)
        {
            try
            {
                var user = _userService.GetByUsername(changes.Username);
                if (_userService.ChangePassword(user, changes.OldPassword, changes.NewPassword))
                {
                    _userService.SaveChanges();
                    return Ok();
                }
                return BadRequest();

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

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var user = _mapper.Map<User>(userViewModel);
                    _userService.Add(user);
                    _userService.SaveChanges();
                    return Ok(user);
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

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] UserViewModel userVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var user = _mapper.Map<User>(userVm);
                    _userService.Update(user);
                    _userService.SaveChanges();
                    return Ok(user);
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

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CBSToken, int id)
        {
            var user = _userService.GetById(id);
            bool IsValid = user != null;
            if (!IsValid) return BadRequest("The input Id doesn't exist!");
            else
            {
                try
                {
                    _userService.Delete(id);
                    _userService.SaveChanges();
                    return Ok("Deleted");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}