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
    [Route("api/screeningposition")]
    [ApiController]
    public class ScreeningPositionController : ControllerBase
    {
        private readonly IScreeningPositionService _screeningPositionService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public ScreeningPositionController(IScreeningPositionService screeningPositionService, IMapper mapper, IErrorService errorService)
        {
            _screeningPositionService = screeningPositionService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listScreeningPosition = _screeningPositionService.GetAll();
            var listScreeningPositionVm = _mapper.Map<IEnumerable<ScreeningPositionViewModel>>(listScreeningPosition);
            return Ok(listScreeningPositionVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var screeningPosition = _screeningPositionService.GetById(id);
            if (screeningPosition == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var screeningPositionVm = _mapper.Map<ScreeningPositionViewModel>(screeningPosition);
                return Ok(screeningPositionVm);
            }
        }

        [HttpGet]
        [Route("getallbyscreening/{id}")]
        public ActionResult GetAllByScreening([FromHeader, Required] string CBSToken, int id)
        {
            var screeningPositions = _screeningPositionService.GetAllByScreening(id);
            if (screeningPositions == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var screeningPositionsVm = _mapper.Map<IEnumerable<ScreeningPositionViewModel>>(screeningPositions);
                return Ok(screeningPositionsVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] ScreeningPositionViewModel screeningPositionVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var screeningPosition = _mapper.Map<ScreeningPosition>(screeningPositionVm);
                    _screeningPositionService.Add(screeningPosition);
                    _screeningPositionService.SaveChanges();
                    return Created("Create successfully", screeningPosition);
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
        [Route("createmulti")]
        public ActionResult PostMulti([FromHeader, Required] string CBSToken, [FromBody] IEnumerable<ScreeningPositionViewModel> screeningPositionVms)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var screeningPositions = _mapper.Map<IEnumerable<ScreeningPosition>>(screeningPositionVms);
                    foreach (var sp in screeningPositions)
                    {
                        _screeningPositionService.Add(sp);
                        _screeningPositionService.SaveChanges();
                    }
                    return Created("Create successfully", screeningPositions);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] ScreeningPositionViewModel screeningPositionVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var screeningPosition = _mapper.Map<ScreeningPosition>(screeningPositionVm);
                    _screeningPositionService.Update(screeningPosition);
                    _screeningPositionService.SaveChanges();
                    return Ok(screeningPositionVm);
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
            var screeningPosition = _screeningPositionService.GetById(id);
            bool IsValid = screeningPosition != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _screeningPositionService.Delete(id);
                    _screeningPositionService.SaveChanges();
                    return Ok("Deleted");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpDelete]
        [Route("deletebyscreening/{id}")]
        public ActionResult DeleteByScreening([FromHeader, Required] string CBSToken, int id)
        {
            try
            {
                _screeningPositionService.DeleteByScreening(id);
                _screeningPositionService.SaveChanges();
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}