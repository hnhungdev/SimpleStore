using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            "[START] Request={RequestName} RequestData={RequestData}",
            requestName,
            request);

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (stopwatch.Elapsed.TotalSeconds > 3)
        {
            _logger.LogWarning(
                "[PERFORMANCE] Request={RequestName} ExecutionTime={ExecutionTime}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
        }

        _logger.LogInformation(
            "[END] Request={RequestName} ExecutionTime={ExecutionTime}ms",
            requestName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }
}