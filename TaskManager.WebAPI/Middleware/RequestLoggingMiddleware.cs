namespace TaskManager.WebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            _logger.LogInformation(
                "Request started: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            try
            {
                await _next(context);

                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogInformation(
                    "Request completed: {Method} {Path} - Status: {StatusCode} - Elapsed: {Elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    elapsed.TotalMilliseconds);
            }
            catch (Exception)
            {
                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogError(
                    "Request failed: {Method} {Path} - Elapsed: {Elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    elapsed.TotalMilliseconds);

                throw;
            }
        }
    }
}
