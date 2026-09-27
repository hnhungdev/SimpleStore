using Notification.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Notification.Infrastructure.Messaging.Inbox;

public sealed class NotificationInboxStore(NotificationDbContext dbContext) : INotificationInboxStore
{
    private readonly DbSet<InboxMessage> _set = dbContext.Set<InboxMessage>();

    public Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default)
    {
        return _set.AsNoTracking()
                   .AnyAsync(x => x.Id == messageId && x.ProcessedAt != null, ct);
    }

    public void Add(InboxMessage message)
    {
        _set.Add(message);
    }
}