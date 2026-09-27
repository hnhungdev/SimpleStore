using System.Threading.Channels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.NotificationJobs.Commands;
using Notification.Application.Features.NotificationJobs.Queries;

public sealed class NotificationWorker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<NotificationWorker> logger) : BackgroundService
{
    private const int BatchSize = 100;
    private const int WorkerDelaySeconds = 5;
    private const int MaxConcurrency = 10;

    private readonly Channel<Guid> _channel =
        Channel.CreateBounded<Guid>(new BoundedChannelOptions(500)
        {
            FullMode = BoundedChannelFullMode.Wait
        });

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Notification Worker started");

        _ = Task.Run(() => ProcessQueue(stoppingToken), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                var jobs = await sender.Send(
                    new GetProcessableJobsQuery(BatchSize),
                    stoppingToken);

                foreach (var job in jobs)
                {
                    await _channel.Writer.WriteAsync(job.Id, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching notification jobs");
            }

            await Task.Delay(TimeSpan.FromSeconds(WorkerDelaySeconds), stoppingToken);
        }
    }

    private async Task ProcessQueue(CancellationToken token)
    {
        var workers = Enumerable.Range(0, MaxConcurrency)
            .Select(_ => Task.Run(() => WorkerLoop(token), token))
            .ToArray();

        await Task.WhenAll(workers);
    }

    private async Task WorkerLoop(CancellationToken token)
    {
        while (await _channel.Reader.WaitToReadAsync(token))
        {
            while (_channel.Reader.TryRead(out var jobId))
            {
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                    await sender.Send(
                        new ProcessNotificationJobCommand(jobId),
                        token);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed processing job {JobId}", jobId);
                }
            }
        }
    }
}