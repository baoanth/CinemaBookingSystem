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
    [Route("api/screening")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public ScreeningController(IScreeningService screeningService, IMapper mapper, IErrorService errorService)
        {
            _screeningService = screeningService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listScreening = _screeningService.GetAll();
            var listScreeningVm = _mapper.Map<IEnumerable<ScreeningViewModel>>(listScreening);
            return Ok(listScreeningVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var screening = _screeningService.GetById(id);
            if (screening == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var screeningVm = _mapper.Map<ScreeningViewModel>(screening);
                return Ok(screeningVm);
            }
        }

        [HttpGet]
        [Route("getallbytheatre/{id}")]
        public ActionResult GetAllByTheatre([FromHeader, Required] string CBSToken, int id)
        {
            var screening = _screeningService.GetAllByTheatre(id);
            if (screening == null) return NotFound("There's no screening schedule!");
            else
            {
                var screeningVm = _mapper.Map<IEnumerable<ScreeningViewModel>>(screening);
                return Ok(screeningVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] ScreeningViewModel screeningVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var screening = _mapper.Map<Screening>(screeningVm);
                    _screeningService.Add(screening);
                    _screeningService.SaveChanges();
                    return Created("Create successfully", screening);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] ScreeningViewModel screeningVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var screening = _mapper.Map<Screening>(screeningVm);
                    _screeningService.Update(screening);
                    _screeningService.SaveChanges();
                    return Ok(screeningVm);
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
            var screening = _screeningService.GetById(id);
            bool IsValid = screening != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _screeningService.Delete(id);
                    _screeningService.SaveChanges();
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