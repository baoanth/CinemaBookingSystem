using AutoMapper;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/bookingdetail")]
    [ApiController]
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public BookingDetailController(IBookingDetailService bookingDetailService, IMapper mapper, IErrorService errorService)
        {
            _bookingDetailService = bookingDetailService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getallbybooking/{bookingId}")]
        public ActionResult GetAllByBooking([FromHeader, Required] string CBSToken,int bookingId)
        {
            var list = _bookingDetailService.GetAllByBooking(bookingId);
            var listVm = _mapper.Map<IEnumerable<BookingDetailViewModel>>(list);
            return Ok(listVm);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] BookingDetailViewModel bookingDetailVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var bookingDetail = _mapper.Map<BookingDetail>(bookingDetailVm);
                    _bookingDetailService.Add(bookingDetail);
                    _bookingDetailService.SaveChanges();
                    return Created("Create successfully", bookingDetail);
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
            bool IsSuccess = _bookingDetailService.DeleteMulti(id);
            if (IsSuccess)
            {
                _bookingDetailService.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}