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
    [Route("api/theatre")]
    [ApiController]
    public class TheatreController : ControllerBase
    {
        private readonly ITheatreService _theatreService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public TheatreController(ITheatreService theatreService, IMapper mapper, IErrorService errorService)
        {
            _theatreService = theatreService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listTheatre = _theatreService.GetAll();
            var listTheatreVm = _mapper.Map<IEnumerable<TheatreViewModel>>(listTheatre);
            return Ok(listTheatreVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var theatre = _theatreService.GetById(id);
            if (theatre == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var theatreVm = _mapper.Map<TheatreViewModel>(theatre);
                return Ok(theatreVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] TheatreViewModel theatreVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var theatre = _mapper.Map<Theatre>(theatreVm);
                    _theatreService.Add(theatre);
                    _theatreService.SaveChanges();
                    return Created("Create successfully", theatreVm);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] TheatreViewModel theatreVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var theatre = _mapper.Map<Theatre>(theatreVm);
                    _theatreService.Update(theatre);
                    _theatreService.SaveChanges();
                    return Ok(theatreVm);
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
            var theatre = _theatreService.GetById(id);
            bool IsValid = theatre != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _theatreService.Delete(id);
                    _theatreService.SaveChanges();
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