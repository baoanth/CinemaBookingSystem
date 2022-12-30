using AutoMapper;
using Azure;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/cinema")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        private readonly IMapper _mapper;

        public CinemaController(ICinemaService cinemaService, IMapper mapper)
        {
            _cinemaService = cinemaService;
            _mapper = mapper;
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
            if (cinema == null) return NotFound();
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
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var cinema = _mapper.Map<Cinema>(cinemaVm);
                _cinemaService.Add(cinema);
                _cinemaService.SaveChanges();
                return Created("Create successfully", cinemaVm);
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CinemaViewModel cinemaVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var cinema = _mapper.Map<Cinema>(cinemaVm);
                _cinemaService.Update(cinema);
                _cinemaService.SaveChanges();
                return Ok(cinemaVm);
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
                _cinemaService.Delete(id);
                _cinemaService.SaveChanges();
                return Ok();
            }
        }
    }
}
