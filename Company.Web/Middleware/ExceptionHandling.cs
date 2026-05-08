using System.Net;
using System.Text.Json;

namespace Company.Web.Middleware
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                UnauthorizedAccessException => new ErrorResponse(
                    (int)HttpStatusCode.Unauthorized,
                    "Unauthorized",
                    "Invalid or missing token."),

                BadHttpRequestException => new ErrorResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Validation Error",
                    "The provided data is invalid."),

                KeyNotFoundException => new ErrorResponse(
                    (int)HttpStatusCode.NotFound,
                    "Not Found",
                    "The requested resource was not found."),

                _ => new ErrorResponse(
                    (int)HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    "An unexpected server error occurred.")
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public record ErrorResponse(int StatusCode, string Error, string Message);
}