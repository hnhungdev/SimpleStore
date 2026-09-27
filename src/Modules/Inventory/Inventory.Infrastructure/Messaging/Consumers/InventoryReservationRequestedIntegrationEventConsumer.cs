using System.Text.Json;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryReservation.Commands;
using Inventory.Application.Features.InventoryReservation.Dtos;
using Inventory.Infrastructure.Messaging.Inbox;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Messaging.Consumers;

public sealed class InventoryReservationRequestedIntegrationEventConsumer(
    ISender sender,
    IInventoryInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<InventoryReservationRequestedIntegrationEventConsumer> logger)
    : IConsumer<InventoryReservationRequestedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<InventoryReservationRequestedIntegrationEvent> context)
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
            eventType: nameof(InventoryReservationRequestedIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var command = BuildCommand(message);

        await sender.Send(command, context.CancellationToken);

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation(
            "Successfully processed inventory reservation for order. " +
            "OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            messageId);
    }

    private static CreateReservationCommand BuildCommand(
        InventoryReservationRequestedIntegrationEvent message)
    {
        var dto = new CreateInventoryReservationDto
        {
            OrderId = message.OrderId,
            Items = message.OrderItems
                .Select(x => new CreateReservationItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                })
                .ToList()
        };

        return new CreateReservationCommand(dto);
    }
}