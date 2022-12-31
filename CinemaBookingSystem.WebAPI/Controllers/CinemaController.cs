using AutoMapper;
using Azure;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/cinema")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public CinemaController(ICinemaService cinemaService, IMapper mapper, IErrorService errorService)
        {
            _cinemaService = cinemaService;
            _mapper = mapper;
            _errorService = errorService;
        }
        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken)
        {
            var listCinema = _cinemaService.GetAll();
            var listCinemaVm = _mapper.Map<IEnumerable<CinemaViewModel>>(listCinema);
            return Ok(listCinemaVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var cinema = _cinemaService.GetById(id);
            if (cinema == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var cinemaVm = _mapper.Map<CinemaViewModel>(cinema);
                return Ok(cinemaVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CinemaViewModel cinemaVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var cinema = _mapper.Map<Cinema>(cinemaVm);
                    _cinemaService.Add(cinema);
                    _cinemaService.SaveChanges();
                    return Created("Create successfully", cinemaVm);
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
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CinemaViewModel cinemaVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var cinema = _mapper.Map<Cinema>(cinemaVm);
                    _cinemaService.Update(cinema);
                    _cinemaService.SaveChanges();
                    return Ok(cinemaVm);
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
        public ActionResult Delete([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var cinema = _cinemaService.GetById(id);
            bool IsValid = cinema != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _cinemaService.Delete(id);
                    _cinemaService.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
