using Microsoft.Extensions.Logging;
using Order.Domain.Events;
using MassTransit;
using MediatR;

namespace Order.Application.Features.Order.EventHandlers;

public sealed class OrderCancelledDomainEventHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderCancelledDomainEventHandler> logger) 
    : INotificationHandler<OrderCancelledDomainEvent>
{
    public async Task Handle(OrderCancelledDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new OrderCancelledIntegrationEvent
        {
            OrderId = @event.OrderId,
            Reason = @event.CancelReason!
        };

        await publishEndpoint.Publish(integrationEvent, ct);

        logger.LogInformation(
            "Published {IntegrationEvent} to Outbox for OrderId: {OrderId}",
            nameof(OrderCancelledIntegrationEvent),
            @event.OrderId);
    }
}