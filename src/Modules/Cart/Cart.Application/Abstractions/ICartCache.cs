using Cart.Application.Features.Cart.Dtos;

namespace Cart.Application.Abstractions;

public interface ICartCache
{
    Task<CartDto?> GetAsync(Guid userId, CancellationToken ct = default);
    Task SetAsync(CartDto cart, CancellationToken ct = default);
    Task RemoveAsync(Guid userId, CancellationToken ct = default);
}