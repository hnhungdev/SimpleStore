namespace Inventory.Application.Features.InventoryItem.Dtos;

public class CreateInventoryItemDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
