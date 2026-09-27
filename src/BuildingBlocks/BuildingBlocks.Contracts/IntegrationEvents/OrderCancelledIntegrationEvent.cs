namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record OrderCancelledIntegrationEvent
{
    public Guid OrderId { get; init; }
    
    public string Reason { get; init; } = string.Empty;
}

