using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TaskScheduler.Exceptions;

namespace SuperAgentCore.Exceptions
{
    public class StatusCodeExceptionHandler
    {
        private readonly RequestDelegate _request;

        public StatusCodeExceptionHandler(RequestDelegate pipeline)
        {
            _request = pipeline;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (StatusCodeException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, StatusCodeException exception)
        {
            var result = JsonConvert.SerializeObject(new { isSuccess = false, error = exception.Description});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.StatusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
