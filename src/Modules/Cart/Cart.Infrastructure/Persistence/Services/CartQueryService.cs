using Cart.Application.Features.Cart.Dtos;
using Microsoft.EntityFrameworkCore;
using Cart.Application.Features.Cart.Services;

namespace Cart.Infrastructure.Persistence.Services;

public sealed class CartQueryService(CartDbContext context) : ICartQueryService
{
   public async Task<CartDto?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Carts
            .Include(x => x.Items)
            .Select(x => new CartDto
            {
                UserId = x.UserId,
                Items = x.Items.Select(item => new CartItemDto
                {
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    ThumbnailUrl = item.Product.ThumbnailUrl,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                }).ToList(),
                TotalPrice = x.Items.Sum(x => x.Product.Price * x.Quantity)
            })
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }
}