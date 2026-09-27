using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Abstractions;
using Notification.Application.Features.NotificationJobs.Commands;
using Notification.Application.Features.NotificationJobs.Dtos;
using Notification.Domain.Constants;
using Notification.Domain.Enums;
using Notification.Infrastructure.Messaging.Inbox;

namespace Notification.Infrastructure.Messaging.Consumers;

public sealed class OrderConfirmedIntegrationEventConsumer(
    ISender sender,
    INotificationInboxStore inboxStore,
    IUnitOfWork unitOfWork,
    ILogger<OrderConfirmedIntegrationEventConsumer> logger)
    : IConsumer<OrderConfirmedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderConfirmedIntegrationEvent> context)
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
            eventType: nameof(OrderConfirmedIntegrationEvent),
            content: JsonSerializer.Serialize(message));

        inboxStore.Add(inboxMessage);

        var templateVariables = new Dictionary<string, object>
        {
            { TemplateVariables.OrderNo, message.OrderNo },
            { TemplateVariables.CustomerName, message.CustomerName },
            { TemplateVariables.OrderTotal, message.TotalPrice },
            { TemplateVariables.OrderUrl, $"/orders/{message.OrderId}" }
        };

        var channels = new[]
        {
            ChannelType.Email,
            ChannelType.InApp
        };

        var notificationJobs = channels
            .Select(channel => new CreateNotificationJobDto
            {
                EventId = messageId,
                UserId = message.UserId,
                TemplateKey = TemplateKey.OrderConfirmed,
                ChannelType = channel,
                Recipient = message.CustomerEmail,
                TemplateVariables = templateVariables,
                Priority = NotificationJobPriority.Medium,
                TargetUrl = $"/orders/{message.OrderId}"
            })
            .ToList();

        await sender.Send(
            new CreateNotificationJobsCommand(notificationJobs),
            context.CancellationToken);

        inboxMessage.Complete(DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(
            context.CancellationToken);

        logger.LogInformation(
            "Successfully created notification jobs for confirmed order. " +
            "OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            messageId);
    }
}