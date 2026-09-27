namespace BuildingBlocks.Contracts.Modules;

public interface ICatalogModule
{
    Task<ProductResponse?> GetProductByIdAsync(Guid productId, CancellationToken ct);
    
    Task<ProductResponse?> GetOrderableProductAsync(Guid productId, CancellationToken ct);

    Task<OrderableProductsResponse> GetOrderableProductsAsync(OrderableProductsRequest request, CancellationToken ct);
}


public sealed record ProductResponse(Guid Id, string Name, string ThumbnailUrl, decimal Price);

public sealed record OrderableProductsRequest(IReadOnlyCollection<Guid> ProductIds);

public sealed record OrderableProductsResponse(IReadOnlyList<ProductResponse> Products);