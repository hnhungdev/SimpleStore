namespace Order.Application.Features.Order.Dtos;

public sealed class UpdateOrderDto
{
    public CustomerDto Customer { get; set; } = default!;

    public string? Notes { get; set; }
}

