using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Messaging.Inbox;

public sealed class InventoryInboxStore(InventoryDbContext dbContext) : IInventoryInboxStore
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