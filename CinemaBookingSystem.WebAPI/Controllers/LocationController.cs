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
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public LocationController(ILocationService locationService, IMapper mapper, IErrorService errorService)
        {
            _locationService = locationService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var locationList = _locationService.GetAll();
            var locationListVm = _mapper.Map<IEnumerable<LocationViewModel>>(locationList);
            return Ok(locationListVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var location = _locationService.GetById(id);
            if (location == null) return NotFound();
            else
            {
                var locationVm = _mapper.Map<LocationViewModel>(location);
                return Ok(locationVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] LocationViewModel locationVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var location = _mapper.Map<Location>(locationVm);
                    _locationService.Add(location);
                    _locationService.SaveChanges();
                    return Created("Create successfully", locationVm);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] LocationViewModel locationVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var location = _mapper.Map<Location>(locationVm);
                    _locationService.Update(location);
                    _locationService.SaveChanges();
                    return Ok(locationVm);
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
            var location = _locationService.GetById(id);
            bool IsValid = location != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _locationService.Delete(id);
                    _locationService.SaveChanges();
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