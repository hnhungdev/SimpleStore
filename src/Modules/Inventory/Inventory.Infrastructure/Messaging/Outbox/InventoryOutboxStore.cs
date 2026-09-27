using System.Text.Json;
using Inventory.Application.Abstractions;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Messaging.Outbox;

public sealed class InventoryOutboxStore(InventoryDbContext dbContext) : IInventoryOutboxStore 
{
    public void Add<TMessage>(TMessage message) where TMessage : class
    {
        var outboxMessage = OutboxMessage.Create(
            typeof(TMessage).AssemblyQualifiedName!,
            JsonSerializer.Serialize(message)
        );

        dbContext.OutboxMessages.Add(outboxMessage);
    }
}