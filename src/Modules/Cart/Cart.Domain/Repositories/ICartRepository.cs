using Cart.Domain.Entities;

namespace Cart.Domain.Repositories;

public interface ICartRepository
{
    void Add(CartEntity cart);

    Task<CartEntity> GetByUserAsync(Guid userId, CancellationToken ct = default);
}
