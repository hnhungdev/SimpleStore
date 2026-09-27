using Cart.Application.Features.Cart.Dtos;
using Cart.Application.Abstractions;
using Cart.Application.Features.Cart.Services;

namespace Cart.Infrastructure.Services;

public sealed class CachedCartQueryService(
    ICartQueryService queryService,
    ICartCache cache)
    : ICartQueryService
{
    public async Task<CartDto?> GetByUserIdAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var cached = await cache.GetAsync(userId, ct);

        if (cached is not null)
            return cached;

        var cart = await queryService.GetByUserIdAsync(userId, ct);

        if (cart is not null)
            await cache.SetAsync(cart, ct);

        return cart;
    }
}