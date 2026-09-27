using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Domain.Events;

namespace Payment.Application.Features.Payment.EventHandlers;

public sealed class PaymentFailedDomainEventHandler(
    IPaymentOutboxStore outboxStore,
    ILogger<PaymentFailedDomainEventHandler> logger)
    : INotificationHandler<PaymentFailedDomainEvent>
{
    public Task Handle(PaymentFailedDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new PaymentFailedIntegrationEvent
        {
            OrderId = @event.OrderId,
            PaymentId = @event.PaymentId,
            FailureReason = @event.FailureReason!
        };

        outboxStore.Add(integrationEvent);

        logger.LogInformation(
            "Added {IntegrationEvent} to Outbox for OrderId {OrderId}, PaymentId {PaymentId}",
            nameof(PaymentFailedIntegrationEvent),
            @event.OrderId,
            @event.PaymentId);

        return Task.CompletedTask;
    }
}