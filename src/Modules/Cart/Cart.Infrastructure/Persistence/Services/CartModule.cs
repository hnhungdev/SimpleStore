using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistence.Services;
public sealed class CartModule(CartDbContext context) : ICartModule
{
    public async Task<CartResponse?> GetCartAsync(Guid userId, CancellationToken ct)
    {
        var cart = await context.Carts
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new CartResponse(
                x.UserId,
                x.Items.Select(item => new CartItemResponse(
                    item.Product.Id,
                    item.Product.Name,
                    item.Product.ThumbnailUrl,
                    item.Quantity,
                    item.Product.Price * item.Quantity)).ToList(),
                x.TotalPrice))
            .FirstOrDefaultAsync(ct);

        return cart;
    }
}