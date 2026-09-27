namespace BuildingBlocks.Contracts.IntegrationEvents;

public record PaymentCreationRequestedIntegrationEvent
{
    public Guid OrderId { get; init; }

    public Guid UserId { get; init; }

    public decimal TotalPrice { get; init; }

    public string PaymentMethod { get; init; } = string.Empty;

    public string ReturnUrl { get; init; } = string.Empty;
}