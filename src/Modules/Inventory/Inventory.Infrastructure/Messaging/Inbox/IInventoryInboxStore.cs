namespace Inventory.Infrastructure.Messaging.Inbox;

public interface IInventoryInboxStore
{
    Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default);

    void Add(InboxMessage message);
}