namespace Inventory.Application.Features.InventoryReservation.Dtos;

public sealed record CreateInventoryReservationDto
{
    public Guid OrderId { get; init; }
    public List<CreateReservationItemDto> Items { get; init; } = new();
}

public sealed class CreateReservationItemDto
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
}

