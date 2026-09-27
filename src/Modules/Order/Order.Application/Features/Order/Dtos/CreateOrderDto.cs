namespace Order.Application.Features.Order.Dtos;

public sealed class CreateOrderDto
{
    public CustomerDto Customer { get; set; } = default!;

    public string PaymentMethod { get; set; } = string.Empty;

    public string? Notes { get; set; }
}

