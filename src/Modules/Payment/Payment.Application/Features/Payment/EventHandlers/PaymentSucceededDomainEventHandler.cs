using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Domain.Events;

namespace Payment.Application.Features.Payment.EventHandlers;

public sealed class PaymentSucceededDomainEventHandler(
    IPaymentOutboxStore outboxStore,
    ILogger<PaymentSucceededDomainEventHandler> logger)
    : INotificationHandler<PaymentSucceededDomainEvent>
{
    public Task Handle(PaymentSucceededDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new PaymentSucceededIntegrationEvent
        {
            OrderId = @event.OrderId,
            PaymentId = @event.PaymentId
        };

        outboxStore.Add(integrationEvent);

        logger.LogInformation(
            "Added {IntegrationEvent} to Outbox for OrderId {OrderId}, PaymentId {PaymentId}",
            nameof(PaymentSucceededIntegrationEvent),
            @event.OrderId,
            @event.PaymentId);

        return Task.CompletedTask;
    }
}