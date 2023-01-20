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
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper, IErrorService errorService)
        {
            _contactService = contactService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var contactList = _contactService.GetAll();
            var contactListVm = _mapper.Map<IEnumerable<ContactViewModel>>(contactList);
            return Ok(contactListVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var contact = _contactService.GetById(id);
            if (contact == null) return BadRequest("The input Id doesn't exist!");
            else
            {
                var contactVm = _mapper.Map<ContactViewModel>(contact);
                return Ok(contactVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] ContactViewModel contactVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var contact = _mapper.Map<Contact>(contactVm);
                    _contactService.Add(contact);
                    _contactService.SaveChanges();
                    return Created("Create successfully", contactVm);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] ContactViewModel contactVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var contact = _mapper.Map<Contact>(contactVm);
                    _contactService.Update(contact);
                    _contactService.SaveChanges();
                    return Ok(contactVm);
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
            var contact = _contactService.GetById(id);
            bool IsValid = contact != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _contactService.Delete(id);
                    _contactService.SaveChanges();
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