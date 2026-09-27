using Notification.Application.Models;
using Notification.Domain.Enums;

namespace Notification.Application.Strategy;

public interface INotificationSender
{
    ChannelType Channel { get; }
    Task<ChannelResult> SendAsync(NotificationContext context, CancellationToken ct);
}
