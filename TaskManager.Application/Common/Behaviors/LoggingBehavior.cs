using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskManager.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation(
                "Handling request: {RequestName} - {@Request}",
                requestName,
                request);

            var startTime = DateTime.UtcNow;

            try
            {
                var response = await next();

                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogInformation(
                    "Request handled: {RequestName} - Elapsed: {Elapsed}ms",
                    requestName,
                    elapsed.TotalMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogError(
                    ex,
                    "Request failed: {RequestName} - Elapsed: {Elapsed}ms - Error: {ErrorMessage}",
                    requestName,
                    elapsed.TotalMilliseconds,
                    ex.Message);

                throw;
            }
        }
    }
}
