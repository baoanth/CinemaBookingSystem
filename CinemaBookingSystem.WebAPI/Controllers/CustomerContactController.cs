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
    [Route("api/customercontact")]
    [ApiController]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactService _customerContactService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public CustomerContactController(ICustomerContactService customerContactService, IMapper mapper, IErrorService errorService)
        {
            _customerContactService = customerContactService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listCustomerContact = _customerContactService.GetAll();
            var listCustomerContactVm = _mapper.Map<IEnumerable<CustomerContactViewModel>>(listCustomerContact);
            return Ok(listCustomerContactVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var customerContact = _customerContactService.GetById(id);
            if (customerContact == null) return NotFound();
            else
            {
                var CustomerContactVm = _mapper.Map<CustomerContactViewModel>(customerContact);
                return Ok(CustomerContactVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] CustomerContactViewModel customerContactVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var customerContact = _mapper.Map<CustomerContact>(customerContactVm);
                    _customerContactService.Add(customerContact);
                    _customerContactService.SaveChanges();
                    return Created("Create successfully", customerContact);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] CustomerContactViewModel customerContactVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var customerContact = _mapper.Map<CustomerContact>(customerContactVm);
                    _customerContactService.Update(customerContact);
                    _customerContactService.SaveChanges();
                    return Ok(customerContact);
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
            if (_customerContactService.GetById(id) == null) return BadRequest("The input ID is not exist!");
            else
            {
                try
                {
                    _customerContactService.Delete(id);
                    _customerContactService.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    _errorService.LogError(ex);
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}