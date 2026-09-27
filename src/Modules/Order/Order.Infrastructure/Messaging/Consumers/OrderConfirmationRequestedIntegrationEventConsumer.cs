using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Features.Order.Commands;
using Order.Application.Features.Order.Dtos;
using Order.Domain.Enums;

namespace Order.Infrastructure.Messaging.Consumers;

public sealed class OrderConfirmationRequestedIntegrationEventConsumer(
    ISender sender,
    ILogger<OrderConfirmationRequestedIntegrationEventConsumer> logger)
    : IConsumer<OrderConfirmationRequestedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderConfirmationRequestedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new UpdateOrderStatusCommand(
            OrderId: message.OrderId,
            new UpdateOrderStatusDto
            {
                Status = OrderStatus.Confirmed,
                Reason = null
            });

        await sender.Send(command, context.CancellationToken);

        logger.LogInformation(
            "Successfully updated order status to Confirmed. OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            context.MessageId);
    }
}