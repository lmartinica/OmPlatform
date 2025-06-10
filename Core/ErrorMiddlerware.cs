using System.Text.Json;

namespace OmPlatform.Core
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle 400 and other error status codes
                if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
                {
                    await WriteError(context, context.Response.StatusCode);
                }
            }
            catch (Exception)
            {
                await WriteError(context, 500);
            }
        }

        private async Task WriteError(HttpContext context, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var errorResponse = new ErrorResponse(statusCode);

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
