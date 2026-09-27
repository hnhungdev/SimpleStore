using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Abstractions;
using Notification.Application.Features.Notification.Dtos;
using Notification.Domain.Enums;
using Notification.Infrastructure.Messaging.Inbox;

public sealed class OrderCreatedIntegrationEventConsumer(
    INotificationHubService notificationHubService,
    INotificationInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<OrderCreatedIntegrationEventConsumer> logger)
    : IConsumer<PaymentCreatedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<PaymentCreatedIntegrationEvent> context)
    {
        var messageId = context.MessageId!.Value;

        var message = context.Message;

        if (await inboxStore.IsProcessedAsync(
                messageId,
                context.CancellationToken))
        {
            logger.LogWarning(
                "Message {MessageId} for UserId {UserId} already processed. Skipping.",
                messageId,
                message.UserId);

            return;
        }

        var inboxMessage = InboxMessage.Create(
            id: messageId,
            eventType: nameof(PaymentCreatedIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var noti = new NotificationDto
        {
            UserId = message.UserId,
            Title = "Thanh toán đơn hàng",
            Message = "Chuyển đến cổng thanh toán",
            TargetUrl = message.PaymentUrl,
            NotificationType = NotificationType.PaymentCreated
        };

        await notificationHubService.SendToUserAsync(
            message.UserId,
            noti,
            context.CancellationToken);

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(
            context.CancellationToken);

        logger.LogInformation(
            "Successfully sent payment notification. " +
            "UserId: {UserId}, MessageId: {MessageId}",
            message.UserId,
            messageId);
    }
}