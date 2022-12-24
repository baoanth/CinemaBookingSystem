using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CinemaBookingSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProvinceController : ApiControllerBase
    {
        private IProvinceService _provinceService;

        public ProvinceController(IErrorService errorService, IProvinceService provinceService) : base(errorService)
        {
            _provinceService = provinceService;
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
                    response = request.CreateResponse(HttpStatusCode.OK, province);
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
                    response = request.CreateResponse(HttpStatusCode.OK, province);
                }
                return response;
            });
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] Province province)
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
                    _provinceService.Add(province);
                    _provinceService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created);
                }
                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, [FromBody] Province province)
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