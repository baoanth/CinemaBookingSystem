using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken, int movieId)
        {
            var listComment = _commentService.GetAll(movieId);
            var listCommentVm = _mapper.Map<IEnumerable<MovieViewModel>>(listComment);
            return Ok(listCommentVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var comment = _commentService.GetById(id);
            if (comment == null) return NotFound();
            else
            {
                var movieVm = _mapper.Map<MovieViewModel>(comment);
                return Ok(movieVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CommentViewModel commentVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var comment = _mapper.Map<Comment>(commentVm);
                _commentService.Add(comment);
                _commentService.SaveChanges();
                return Created("Create successfully", commentVm);
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] CommentViewModel commentVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var comment = _mapper.Map<Comment>(commentVm);
                _commentService.Update(comment);
                _commentService.SaveChanges();
                return Ok(commentVm);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var comment = _commentService.GetById(id);
            bool IsValid = comment != null;
            if (!IsValid) return BadRequest();
            else
            {
                _commentService.Delete(id);
                _commentService.SaveChanges();
                return Ok();
            }
        }
    }
}
