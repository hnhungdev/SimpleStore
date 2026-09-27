namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record PaymentCreatedIntegrationEvent
{
    public Guid UserId { get; init; }
    
    public Guid PaymentId { get; init; }

    public Guid OrderId { get; init; }
    
    public string PaymentUrl { get; init; } = string.Empty;

    public DateTimeOffset? ExpiresAt { get; init; }
}