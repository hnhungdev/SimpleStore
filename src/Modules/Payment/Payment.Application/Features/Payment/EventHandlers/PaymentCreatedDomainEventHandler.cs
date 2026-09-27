using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Domain.Events;

namespace Payment.Application.Features.Payment.EventHandlers;

public sealed class PaymentCreatedDomainEventHandler(
    IPaymentOutboxStore outboxStore,
    ILogger<PaymentCreatedDomainEventHandler> logger)
    : INotificationHandler<PaymentCreatedDomainEvent>
{
    public Task Handle(PaymentCreatedDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new PaymentCreatedIntegrationEvent
        {
            UserId = @event.UserId,
            OrderId = @event.OrderId,
            PaymentId = @event.PaymentId,
            PaymentUrl = @event.PaymentUrl,
            ExpiresAt = @event.ExpiresAt
        };

        outboxStore.Add(integrationEvent);

        logger.LogInformation(
            "Added {IntegrationEvent} to Outbox for OrderId {OrderId}, PaymentId {PaymentId}",
            nameof(PaymentCreatedIntegrationEvent),
            @event.OrderId,
            @event.PaymentId);

        return Task.CompletedTask;
    }
}