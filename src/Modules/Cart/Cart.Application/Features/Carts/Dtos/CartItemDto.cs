namespace Cart.Application.Features.Cart.Dtos;

public class CartItemDto
{
    public Guid ProductId { get; set; } 

    public string ProductName { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public int Quantity { get; set; } 

    public decimal Price { get; set; } 
}
