using Cart.Application.Features.Cart.Dtos;
using Cart.Application.Features.Cart.Services;

namespace Cart.Application.Features.Cart.Queries;

public sealed record GetCartQuery(Guid UserId) : IQuery<CartDto>;

public sealed class GetCartQueryHandler(ICartQueryService cartQueryService)
 : IQueryHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery query, CancellationToken ct)
    {
        var cart = await cartQueryService.GetByUserIdAsync(query.UserId, ct)
            ?? new CartDto();

        return cart;
    }
}