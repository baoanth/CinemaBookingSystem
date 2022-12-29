using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.Infrastructure.Core;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/province")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly IApiControllerBase _base;
        private readonly IMapper _mapper;

        public ProvinceController(IProvinceService provinceService, IMapper mapper, IApiControllerBase @base)
        {
            _provinceService = provinceService;
            _mapper = mapper;
            _base = @base;
        }

        [HttpGet]
        [Route("getall")]
        public ActionResult Get([FromHeader, Required] string CinemaBookingSystemToken)
        {
            var listProvince = _provinceService.GetAll();
            var provinceVm = _mapper.Map<IEnumerable<ProvinceViewModel>>(listProvince);
            return Ok(provinceVm);
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public ActionResult GetSingle([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var province = _provinceService.GetByID(id);
            if (province == null) return NotFound();
            else
            {
                var provinceVm = _mapper.Map<ProvinceViewModel>(province);
                return Ok(provinceVm);
            }
        }

        [HttpGet]
        [Route("getbyregion")]
        public ActionResult GetByRegion([FromHeader, Required] string CinemaBookingSystemToken, string region)
        {
            var province = _provinceService.GetByRegion(region);
            if (province == null) return NotFound();
            else
            {
                var provinceVm = _mapper.Map<IEnumerable<ProvinceViewModel>>(province);
                return Ok(provinceVm);
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Post([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] ProvinceViewModel provinceVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            else
            {
                var province = _mapper.Map<Province>(provinceVm);
                _provinceService.Add(province);
                _provinceService.SaveChanges();
                return Created("Create successfully", provinceVm);
            }
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Put([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] ProvinceViewModel provinceVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            else
            {
                var province = _mapper.Map<Province>(provinceVm);
                _provinceService.Update(province);
                _provinceService.SaveChanges();
                return Ok(provinceVm);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete([FromHeader, Required] string CinemaBookingSystemToken, int id)
        {
            var province = _provinceService.GetByID(id);
            bool IsValid = province != null;
            if (!IsValid) return BadRequest("The Id is not exist!");
            else
            {
                _provinceService.Delete(id);
                _provinceService.SaveChanges();
                return Ok();
            }
        }
    }
}