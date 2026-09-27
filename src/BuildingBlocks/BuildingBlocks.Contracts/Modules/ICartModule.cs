namespace BuildingBlocks.Contracts.Modules;

public interface ICartModule
{
    Task<CartResponse?> GetCartAsync(Guid userId, CancellationToken ct);
}

public sealed record CartResponse(Guid UserId, IReadOnlyList<CartItemResponse> Items, decimal TotalPrice);

public sealed record CartItemResponse(Guid ProductId, string ProductName, string ThumbnailUrl, int Quantity, decimal Price);