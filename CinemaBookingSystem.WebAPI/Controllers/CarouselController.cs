using AutoMapper;
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
    [Route("api/carousel")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly ICarouselService _carouselService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public CarouselController(ICarouselService carouselService, IMapper mapper, IErrorService errorService)
        {
            _carouselService = carouselService;
            _mapper = mapper;
            _errorService = errorService;
        }
        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken)
        {
            var listCarousel = _carouselService.GetAll();
            var listCarouselVm = _mapper.Map<IEnumerable<CarouselViewModel>>(listCarousel);
            return Ok(listCarouselVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var carousel = _carouselService.GetById(id);
            if (carousel == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var carouselVm = _mapper.Map<CarouselViewModel>(carousel);
                return Ok(carouselVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CarouselViewModel carouselVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var carousel = _mapper.Map<Carousel>(carouselVm);
                    _carouselService.Add(carousel);
                    _carouselService.SaveChanges();
                    return Created("Create successfully", carouselVm);
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
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CarouselViewModel carouselVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var carousel = _mapper.Map<Carousel>(carouselVm);
                    _carouselService.Update(carousel);
                    _carouselService.SaveChanges();
                    return Ok(carouselVm);
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
            var carousel = _carouselService.GetById(id);
            bool IsValid = carousel != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _carouselService.Delete(id);
                    _carouselService.SaveChanges();
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
