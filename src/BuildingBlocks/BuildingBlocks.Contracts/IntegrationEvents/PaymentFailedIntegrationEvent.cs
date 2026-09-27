namespace BuildingBlocks.Contracts.IntegrationEvents;

public record PaymentFailedIntegrationEvent
{
    public Guid PaymentId { get; init; }

    public Guid OrderId { get; init; }

    public string FailureReason { get; init; } = string.Empty;
}