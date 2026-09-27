using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Domain.Constants;
using Payment.Domain.Repositories;
using Payment.Infrastructure.Messaging.Inbox;

namespace Payment.Infrastructure.Messaging.Consumers;

public sealed class OrderCancelledIntegrationEventConsumer(
    IPaymentRepository paymentRepository,
    IPaymentInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<OrderCancelledIntegrationEventConsumer> logger)
    : IConsumer<OrderCancelledIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderCancelledIntegrationEvent> context)
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
            eventType: nameof(OrderCancelledIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var payment = await paymentRepository.GetByOrderIdAsync(
                message.OrderId,
                context.CancellationToken)
            ?? throw new NotFoundException(
                PaymentErrorCode.PaymentNotFound,
                message.OrderId);

        payment.MarkCancelled();

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation(
            "Successfully marked payment as cancelled. " +
            "OrderId: {OrderId}, PaymentId: {PaymentId}, MessageId: {MessageId}",
            message.OrderId,
            payment.Id,
            messageId);
    }
}