using System.Net;
using System.Text.Json;
using Serilog;

namespace WebApplication2.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ArgumentException argEx:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = argEx.Message });
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { error = "Unauthorized access" });
                    break;
                case KeyNotFoundException keyEx:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = keyEx.Message });
                    break;
                default:
                    result = JsonSerializer.Serialize(new { error = "An error occurred while processing your request" });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}


