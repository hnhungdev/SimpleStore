using Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Services;

public sealed class CatalogModule : ICatalogModule
{
    private readonly CatalogDbContext _context;

    public CatalogModule(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId, CancellationToken ct)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Select(x => new ProductResponse(x.Id, x.Name, x.ThumbnailUrl, x.Price))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ProductResponse?> GetOrderableProductAsync(Guid productId, CancellationToken ct)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.Status == ProductStatus.Published && x.Id == productId)
            .Select(x => new ProductResponse(x.Id, x.Name, x.ThumbnailUrl, x.Price))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<OrderableProductsResponse> GetOrderableProductsAsync(OrderableProductsRequest request, CancellationToken ct)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(x => request.ProductIds.Contains(x.Id) && x.Status == ProductStatus.Published)
            .Select(x => new ProductResponse(x.Id, x.Name, x.ThumbnailUrl, x.Price))
            .ToListAsync(ct);

        return new OrderableProductsResponse(products);
    }
}