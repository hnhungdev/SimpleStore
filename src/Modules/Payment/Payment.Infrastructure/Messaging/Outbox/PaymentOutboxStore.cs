using System.Text.Json;
using Payment.Application.Abstractions;
using Payment.Infrastructure.Persistence;

namespace Payment.Infrastructure.Messaging.Outbox;

public sealed class PaymentOutboxStore(PaymentDbContext dbContext) : IPaymentOutboxStore 
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