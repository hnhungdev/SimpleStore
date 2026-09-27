namespace Payment.Infrastructure.Messaging.Inbox;

public interface IPaymentInboxStore
{
    Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default);

    void Add(InboxMessage message);
}