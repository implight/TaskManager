using System.Net;
using System.Text.Json;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Domain.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace TaskManager.WebAPI.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, title, detail) = GetExceptionDetails(exception);

            context.Response.StatusCode = (int)statusCode;

            var response = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path,
                Type = GetProblemType(statusCode)
            };

            if (_env.IsDevelopment())
            {
                response.Extensions.Add("exception", exception.Message);
                response.Extensions.Add("stackTrace", exception.StackTrace);
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private (HttpStatusCode statusCode, string title, string detail) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                ValidationException validationEx =>
                    (HttpStatusCode.BadRequest, "Validation Error", validationEx.Message),
                NotFoundException notFoundEx =>
                    (HttpStatusCode.NotFound, "Not Found", notFoundEx.Message),
                DomainException domainEx =>
                    (HttpStatusCode.BadRequest, "Domain Error", domainEx.Message),
                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "Unauthorized", "Access denied"),
                _ =>
                    (HttpStatusCode.InternalServerError,
                     "Internal Server Error",
                     _env.IsDevelopment() ? exception.Message : "An error occurred")
            };
        }

        private string GetProblemType(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                HttpStatusCode.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
                HttpStatusCode.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                HttpStatusCode.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                HttpStatusCode.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        }

        private class ProblemDetails
        {
            public int Status { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Detail { get; set; } = string.Empty;
            public string Instance { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public Dictionary<string, object> Extensions { get; set; } = new();
        }
    }
}

