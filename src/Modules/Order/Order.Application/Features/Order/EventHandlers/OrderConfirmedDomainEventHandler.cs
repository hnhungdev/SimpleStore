using Microsoft.Extensions.Logging;
using Order.Domain.Events;
using MassTransit;
using MediatR;

namespace Order.Application.Features.Order.EventHandlers;

public sealed class OrderConfirmedDomainEventHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderConfirmedDomainEventHandler> logger) 
    : INotificationHandler<OrderConfirmedDomainEvent>
{
    public async Task Handle(OrderConfirmedDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new OrderConfirmedIntegrationEvent
        {
            OrderId = @event.OrderId,
            OrderNo = @event.OrderNo,
            UserId = @event.UserId,
            CustomerEmail = @event.CustomerEmail,
            CustomerName = @event.CustomerName,
            TotalPrice = @event.TotalPrice,
            PaymentMethod = (int)@event.PaymentMethod,
            ConfirmedAt = @event.ConfirmedAt
        };

        await publishEndpoint.Publish(integrationEvent, ct);

        logger.LogInformation(
            "Published {IntegrationEvent} to Outbox for OrderId: {OrderId}, OrderNo: {OrderNo}",
            nameof(OrderConfirmedIntegrationEvent),
            @event.OrderId,
            @event.OrderNo);
    }
}