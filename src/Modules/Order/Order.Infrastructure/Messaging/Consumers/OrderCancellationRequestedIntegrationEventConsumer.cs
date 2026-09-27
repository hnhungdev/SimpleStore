using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Features.Order.Commands;
using Order.Application.Features.Order.Dtos;
using Order.Domain.Enums;

namespace Order.Infrastructure.Messaging.Consumers;

public sealed class OrderCancellationRequestedIntegrationEventConsumer(
    ISender sender,
    ILogger<OrderCancellationRequestedIntegrationEventConsumer> logger)
    : IConsumer<OrderCancellationRequestedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderCancellationRequestedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new UpdateOrderStatusCommand(
            OrderId: message.OrderId,
            new UpdateOrderStatusDto
            {
                Status = OrderStatus.Canceled,
                Reason = message.Reason
            });

        await sender.Send(command, context.CancellationToken);

        logger.LogInformation(
            "Successfully updated order status to Canceled. OrderId: {OrderId}, MessageId: {MessageId}",
            message.OrderId,
            context.MessageId);
    }
}