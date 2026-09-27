using Microsoft.Extensions.Logging;
using Order.Domain.Events;
using MassTransit;
using MediatR;

namespace Order.Application.Features.Order.EventHandlers;

public sealed class OrderCreatedDomainEventHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderCreatedDomainEventHandler> logger) 
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new OrderCreatedIntegrationEvent
        {
            OrderId = @event.OrderId,
            UserId = @event.UserId,
            OrderItems = @event.OrderItems.Select(oi => new OrderItemIntegrationEvent
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity
            }).ToList(),
            TotalPrice = @event.TotalPrice,
            PaymentMethod = @event.PaymentMethod
        };

        await publishEndpoint.Publish(integrationEvent, ct);

        logger.LogInformation(
            "Published {IntegrationEvent} to Outbox for OrderId: {OrderId}",
            nameof(OrderCreatedIntegrationEvent),
            @event.OrderId);
    }
}