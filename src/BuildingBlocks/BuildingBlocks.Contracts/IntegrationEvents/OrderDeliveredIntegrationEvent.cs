namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record class OrderDeliveredIntegrationEvent 
{
    public Guid OrderId { get; init; }
}

