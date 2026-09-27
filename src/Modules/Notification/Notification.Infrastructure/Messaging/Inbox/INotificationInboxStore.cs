namespace Notification.Infrastructure.Messaging.Inbox;

public interface INotificationInboxStore
{
    Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default);

    void Add(InboxMessage message);
}