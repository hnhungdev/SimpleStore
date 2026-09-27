namespace Inventory.Application.Features.InventoryReservation.Dtos;

public sealed class InventoryReservationFilter
{
    public Guid? OrderId { get; set; }
    public Guid? InventoryItemId { get; set; }
}