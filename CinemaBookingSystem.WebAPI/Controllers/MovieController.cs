using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MovieController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken)
        {
            var listMovie = _movieService.GetAll();
            var listMovieVm = _mapper.Map<IEnumerable<MovieViewModel>>(listMovie);
            return Ok(listMovieVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var movie = _movieService.GetById(id);
            if (movie == null) return NotFound();
            else
            {
                var movieVm = _mapper.Map<MovieViewModel>(movie);
                return Ok(movieVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] MovieViewModel movieVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var movie = _mapper.Map<Movie>(movieVm);
                _movieService.Add(movie);
                _movieService.SaveChanges();
                return Created("Create successfully", movieVm);
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] MovieViewModel movieVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var movie = _mapper.Map<Movie>(movieVm);
                _movieService.Update(movie);
                _movieService.SaveChanges();
                return Ok(movieVm);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var movie = _movieService.GetById(id);
            bool IsValid = movie != null;
            if (!IsValid) return BadRequest();
            else
            {
                _movieService.Delete(id);
                _movieService.SaveChanges();
                return Ok();
            }
        }
    }
}
