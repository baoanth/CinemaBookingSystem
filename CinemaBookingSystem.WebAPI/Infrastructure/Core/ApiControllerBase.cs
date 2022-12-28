using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Web.Http;

namespace CinemaBookingSystem.WebAPI.Infrastructure.Core
{
    public interface IApiControllerBase
    {
        HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> messageFunc);
    }

    public class ApiControllerBase : ApiController, IApiControllerBase
    {
        private IErrorService _errorService;

        public ApiControllerBase(IErrorService errorService)
        {
            _errorService = errorService;
        }

        public HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> messageFunc)
        {
            // Arrange.
            var configuration = new HttpConfiguration();
            requestMessage.SetConfiguration(configuration);

            HttpResponseMessage response = null;
            try
            {
                response = messageFunc.Invoke();
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
                LogError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (DbUpdateException dbEx)
            {
                LogError(dbEx);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, dbEx.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return response;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                _errorService.Create(error);
                _errorService.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}