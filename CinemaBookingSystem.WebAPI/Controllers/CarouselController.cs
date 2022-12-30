using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/carousel")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly ICarouselService _carouselService;
        private readonly IMapper _mapper;

        public CarouselController(ICarouselService carouselService, IMapper mapper)
        {
            _carouselService = carouselService;
            _mapper = mapper;
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
            if (carousel == null) return NotFound();
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
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var carousel = _mapper.Map<Carousel>(carouselVm);
                _carouselService.Add(carousel);
                _carouselService.SaveChanges();
                return Created("Create successfully", carouselVm);
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CarouselViewModel carouselVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var carousel = _mapper.Map<Carousel>(carouselVm);
                _carouselService.Update(carousel);
                _carouselService.SaveChanges();
                return Ok(carouselVm);
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
                _carouselService.Delete(id);
                _carouselService.SaveChanges();
                return Ok();
            }
        }
    }
}
