using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Abstractions;
using Payment.Domain.Events;

namespace Payment.Application.Features.Payment.EventHandlers;

public sealed class PaymentCreationFailedDomainEventHandler(
    IPaymentOutboxStore outboxStore,
    ILogger<PaymentCreationFailedDomainEventHandler> logger)
    : INotificationHandler<PaymentCreationFailedDomainEvent>
{
    public Task Handle(PaymentCreationFailedDomainEvent @event, CancellationToken ct)
    {
        var integrationEvent = new PaymentCreationFailedIntegrationEvent
        {
            OrderId = @event.OrderId,
            FailureReason = @event.FailureReason
        };

        outboxStore.Add(integrationEvent);

        logger.LogInformation(
            "Added {IntegrationEvent} to Outbox for OrderId {OrderId}",
            nameof(PaymentCreationFailedIntegrationEvent),
            @event.OrderId);

        return Task.CompletedTask;
    }
}