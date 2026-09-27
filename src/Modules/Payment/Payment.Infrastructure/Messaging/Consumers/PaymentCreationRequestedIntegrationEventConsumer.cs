using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Application.Features.Payment.Command;
using Payment.Infrastructure.Messaging.Inbox;

namespace Payment.Infrastructure.Messaging.Consumers;

public sealed class PaymentCreationRequestedIntegrationEventConsumer(
    ISender sender,
    IPaymentInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<PaymentCreationRequestedIntegrationEventConsumer> logger)
    : IConsumer<PaymentCreationRequestedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<PaymentCreationRequestedIntegrationEvent> context)
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
            eventType: nameof(PaymentCreationRequestedIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var command = new CreatePaymentCommand(
            OrderId: message.OrderId,
            UserId: message.UserId,
            Amount: message.TotalPrice,
            PaymentMethod: message.PaymentMethod);

        await sender.Send(command, context.CancellationToken);

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation(
            "Successfully requested payment creation for OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            messageId);
    }
}