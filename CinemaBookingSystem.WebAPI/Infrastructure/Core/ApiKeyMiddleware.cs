using CinemaBookingSystem.WebAPI.ViewModels;

namespace CinemaBookingSystem.WebAPI.Infrastructure.Core
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "CinemaBookingSystemToken";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token không đúng hoặc đã hết hạn!");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEY);
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token không đúng hoặc đã hết hạn!");
                return;
            }
            await _next(context);
        }
    }
}