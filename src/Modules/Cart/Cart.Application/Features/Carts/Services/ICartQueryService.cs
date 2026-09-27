using Cart.Application.Features.Cart.Dtos;

namespace Cart.Application.Features.Cart.Services;

public interface ICartQueryService
{
    Task<CartDto?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}