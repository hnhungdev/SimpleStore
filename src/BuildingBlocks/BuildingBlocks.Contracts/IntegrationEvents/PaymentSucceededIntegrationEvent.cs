namespace BuildingBlocks.Contracts.IntegrationEvents;

public record PaymentSucceededIntegrationEvent
{
    public Guid PaymentId { get; init; }

    public Guid OrderId { get; init; }
}