using Microsoft.Extensions.Logging;
using Order.Domain.Events;
using MassTransit;
using MediatR;

namespace Order.Application.Features.Order.EventHandlers;

public sealed class OrderDeliveredDomainEventHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderDeliveredDomainEventHandler> logger) 
    : INotificationHandler<OrderDeliveredDomainEvent>
{
    public async Task Handle(OrderDeliveredDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new OrderDeliveredIntegrationEvent
        {
            OrderId = @event.OrderId
        };

        await publishEndpoint.Publish(integrationEvent, ct);

        logger.LogInformation(
            "Published {IntegrationEvent} to Outbox for OrderId: {OrderId}",
            nameof(OrderDeliveredIntegrationEvent),
            @event.OrderId);
    }
}