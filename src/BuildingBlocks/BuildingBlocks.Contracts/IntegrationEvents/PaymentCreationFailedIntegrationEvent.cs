namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record PaymentCreationFailedIntegrationEvent
{
    public Guid OrderId { get; init; }

    public string FailureReason { get; init; } = string.Empty;
}