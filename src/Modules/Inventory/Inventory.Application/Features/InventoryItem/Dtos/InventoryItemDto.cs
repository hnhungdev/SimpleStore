namespace Inventory.Application.Features.InventoryItem.Dtos;

public class InventoryItemDto
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int OnHand { get; set; }

    public int Reserved { get; set; }

    public int Available => OnHand - Reserved;
}
