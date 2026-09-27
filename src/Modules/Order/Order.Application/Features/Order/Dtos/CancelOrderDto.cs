namespace Order.Application.Features.Order.Dtos;

public sealed class CancelOrderDto
{
    public string Reason { get; set; } = string.Empty;
}