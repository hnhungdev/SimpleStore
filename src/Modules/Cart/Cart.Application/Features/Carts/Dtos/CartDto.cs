namespace Cart.Application.Features.Cart.Dtos;

public sealed class CartDto
{
    public Guid UserId { get; set; } 

    public IReadOnlyList<CartItemDto> Items { get; set; } = new List<CartItemDto>();

    public decimal TotalPrice { get; set; }
}
