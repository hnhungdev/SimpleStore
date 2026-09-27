using System.Text.Json;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryReservation.Commands;
using Inventory.Infrastructure.Messaging.Inbox;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Messaging.Consumers;

public sealed class OrderDeliveredIntegrationEventConsumer(
    ISender sender,
    IInventoryInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<OrderDeliveredIntegrationEventConsumer> logger)
    : IConsumer<OrderDeliveredIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderDeliveredIntegrationEvent> context)
    {
        var messageId = context.MessageId!.Value;

        var message = context.Message;

        if (await inboxStore.IsProcessedAsync(
                messageId,
                context.CancellationToken))
        {
            logger.LogWarning(
                "Message {MessageId} for OrderId {OrderId} already processed. Skipping.",
                messageId,
                message.OrderId);

            return;
        }

        var inboxMessage = InboxMessage.Create(
            id: messageId,
            eventType: nameof(OrderDeliveredIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var commitCommand = new CommitReservationCommand(message.OrderId);

        await sender.Send(commitCommand, context.CancellationToken);

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(
            context.CancellationToken);

        logger.LogInformation(
            "Successfully committed inventory reservation for delivered order. " +
            "OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            messageId);
    }
}