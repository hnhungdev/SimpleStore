namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record class OrderConfirmationRequestedIntegrationEvent
{
    public Guid OrderId { get; init; }
}

