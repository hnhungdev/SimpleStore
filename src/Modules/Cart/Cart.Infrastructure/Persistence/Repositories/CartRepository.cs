using Cart.Domain.Repositories;
using Cart.Domain.Entities;
using Cart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Repositories;

public sealed class CartRepository(CartDbContext context) : ICartRepository
{
    private readonly DbSet<CartEntity> _dbSet = context.Set<CartEntity>();

    public void Add(CartEntity cart)
    {
        _dbSet.AddAsync(cart);
    }

    public async Task<CartEntity> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await _dbSet
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);

        if (cart is not null)
            return cart;

        cart = CartEntity.Create(userId);

        Add(cart);

        return cart;
    }
}

