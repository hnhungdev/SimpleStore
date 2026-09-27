using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryReservation.Dtos;

public sealed record InventoryReservationDto
{
    public Guid Id { get; init; }

    public Guid InventoryItemId { get; init; }

    public Guid OrderId { get; init; }

    public int Quantity { get; init; }

    public ReservationStatus Status { get; init; }

    public string DisplayStatus => Status.ToString();

    public DateTimeOffset CreatedAt { get; init; }
}

