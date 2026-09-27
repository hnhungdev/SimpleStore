using System.Text.Json;
using Cart.Application.Features.Cart.Dtos;
using Cart.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Cart.Infrastructure.Services;

public sealed class CartCache : ICartCache
{
    private readonly IDistributedCache _cache;
    private static readonly DistributedCacheEntryOptions Options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
        SlidingExpiration = TimeSpan.FromHours(1)
    };

    public CartCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<CartDto?> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var key = GetKey(userId);

        var data = await _cache.GetStringAsync(key, ct);

        return data is null
            ? null
            : JsonSerializer.Deserialize<CartDto>(data);
    }

    public async Task SetAsync(CartDto cart, CancellationToken ct = default)
    {
        var key = GetKey(cart.UserId);

        var data = JsonSerializer.Serialize(cart);

        await _cache.SetStringAsync(key, data, Options, ct);
    }

    public Task RemoveAsync(Guid userId, CancellationToken ct = default)
        => _cache.RemoveAsync(GetKey(userId), ct);

    private static string GetKey(Guid userId)
        => $"cart:{userId}";
}