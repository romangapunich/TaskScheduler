using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskScheduler.MediatR.Exceptions;

namespace TaskScheduler.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception.GetType() == typeof(ValidationException))
            {
                var code = HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(((ValidationException)exception).ResponeVm, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);

            }
            else
            {
                var code = HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(new { isSuccess = false, error = exception.Message + "/// " + exception.HelpLink + "/// " + exception.InnerException + "/// " + exception.TargetSite });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }
        }
    }
}
