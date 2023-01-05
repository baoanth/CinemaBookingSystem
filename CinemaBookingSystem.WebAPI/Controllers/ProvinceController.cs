using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/province")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public ProvinceController(IProvinceService provinceService, IMapper mapper, IErrorService errorService)
        {
            _provinceService = provinceService;
            _mapper = mapper;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CBSToken)
        {
            var listProvince = _provinceService.GetAll();
            var provinceVm = _mapper.Map<IEnumerable<ProvinceViewModel>>(listProvince);
            return Ok(provinceVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CBSToken, int id)
        {
            var province = _provinceService.GetById(id);
            if (province == null) return NotFound();
            else
            {
                var provinceVm = _mapper.Map<ProvinceViewModel>(province);
                return Ok(provinceVm);
            }
        }

        [HttpGet]
        [Route("getbyregion")]
        public ActionResult GetByRegion([FromHeader, Required] string CBSToken, string region)
        {
            var listProvince = _provinceService.GetByRegion(region);
            if (listProvince.Count() <= 0) return NotFound();
            else
            {
                var listProvinceVm = _mapper.Map<IEnumerable<ProvinceViewModel>>(listProvince);
                return Ok(listProvinceVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CBSToken, [FromBody] ProvinceViewModel provinceVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var province = _mapper.Map<Province>(provinceVm);
                    _provinceService.Add(province);
                    _provinceService.SaveChanges();
                    return Created("Create successfully", provinceVm);
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
        public ActionResult Put([FromHeader, Required] string CBSToken, [FromBody] ProvinceViewModel provinceVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            else
            {
                try
                {
                    var province = _mapper.Map<Province>(provinceVm);
                    _provinceService.Update(province);
                    _provinceService.SaveChanges();
                    return Ok(provinceVm);
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
            var province = _provinceService.GetById(id);
            bool IsValid = province != null;
            if (!IsValid) return BadRequest();
            else
            {
                try
                {
                    _provinceService.Delete(id);
                    _provinceService.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}