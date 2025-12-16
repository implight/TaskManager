using TaskManager.WebAPI.Middleware;

namespace TaskManager.WebAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            return app;
        }
    }
}
