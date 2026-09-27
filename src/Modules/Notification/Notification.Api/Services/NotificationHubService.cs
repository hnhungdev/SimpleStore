using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Api.Hubs;
using Notification.Application.Abstractions;
using Notification.Application.Features.Notification.Dtos;

namespace Notification.Api.Services;

public sealed class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    private readonly ILogger<NotificationHubService> _logger;

    private const string UserGroupPrefix = "user_";

    private const string ReceiveNotificationMethod = "ReceiveNotification";

    public NotificationHubService(
        IHubContext<NotificationHub> hubContext,
        ILogger<NotificationHubService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task BroadcastNotificationAsync(NotificationDto notification, CancellationToken ct = default)
    {
        try
        {
            await _hubContext.Clients.All.SendAsync(ReceiveNotificationMethod, notification, ct);
            _logger.LogInformation("Broadcast notification sent: {Title}", notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting notification: {Title}", notification.Title);
        }
    }

    public async Task SendToUserAsync(Guid userId, NotificationDto notification, CancellationToken ct = default)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Cannot send notification to user: userId is null or empty");
                return;
            }

            notification.UserId = userId;
            var groupName = $"{UserGroupPrefix}{userId}";
            Console.WriteLine($"Sending to {groupName}");
            
            await _hubContext.Clients.Group(groupName).SendAsync(ReceiveNotificationMethod, notification, ct);
            _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}: {Title}", userId, notification.Title);
        }
    }

    public async Task SendToUsersAsync(List<Guid> userIds, NotificationDto notification, CancellationToken ct = default)
    {
        try
        {
            if (userIds == null || userIds.Count == 0)
            {
                _logger.LogWarning("Cannot send notification to users: userIds list is null or empty");
                return;
            }

            var tasks = userIds.Select(userId =>
            {
                var groupName = $"{UserGroupPrefix}{userId}";
                return _hubContext.Clients.Group(groupName).SendAsync(ReceiveNotificationMethod, notification, ct);
            });

            await Task.WhenAll(tasks);
            _logger.LogInformation("Notification sent to {Count} users: {Title}", userIds.Count, notification.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to multiple users: {Title}", notification.Title);
        }
    }
}
