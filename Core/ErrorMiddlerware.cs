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
                    context.Response.WriteAsync("TODO");
                }
            }
            catch (Exception ex)
            {
                context.Response.WriteAsync("TODO");
            }
        }
    }
}
