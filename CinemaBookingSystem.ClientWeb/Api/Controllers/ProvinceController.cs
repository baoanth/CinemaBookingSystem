using AutoMapper;
using CinemaBookingSystem.ClientWeb.Infrastructure.Core;
using CinemaBookingSystem.ClientWeb.Models;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CinemaBookingSystem.ClientWeb.Api.Controllers
{
    [Route("api/province")]
    public class ProvinceController : ApiControllerBase
    {
        private IProvinceService _provinceService;
        private readonly IMapper _mapper;

        public ProvinceController(IErrorService errorService, IProvinceService provinceService, IMapper mapper) : base(errorService)
        {
            _provinceService = provinceService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var provinceList = _provinceService.GetAll();
                    var provinceListVm = _mapper.Map<List<ProvinceViewModel>>(provinceList);
                    response = request.CreateResponse(HttpStatusCode.OK, provinceList);
                }
                return response;
            });
        }

        [HttpGet]
        [Route("getsingle/{id}")]
        public HttpResponseMessage GetSingle(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var province = _provinceService.GetByID(id);
                    var provinceVm = _mapper.Map<ProvinceViewModel>(province);
                    response = request.CreateResponse(HttpStatusCode.OK, provinceVm);
                }
                return response;
            });
        }

        [HttpGet]
        [Route("getbyregion")]
        public HttpResponseMessage GetByRegion(HttpRequestMessage request, string region)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var province = _provinceService.GetByRegion(region);
                    var provinceVm = _mapper.Map<ProvinceViewModel>(province);
                    response = request.CreateResponse(HttpStatusCode.OK, provinceVm);
                }
                return response;
            });
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] ProvinceViewModel provinceVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var province = _mapper.Map<Province>(provinceVm);
                    _provinceService.Add(province);
                    _provinceService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created);
                }
                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, [FromBody] ProvinceViewModel provinceVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var province = _mapper.Map<Province>(provinceVm);
                    _provinceService.Update(province);
                    _provinceService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _provinceService.Delete(id);
                    _provinceService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
    }
}
