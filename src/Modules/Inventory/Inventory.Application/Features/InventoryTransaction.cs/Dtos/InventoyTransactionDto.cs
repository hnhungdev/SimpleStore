using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryTransaction.Dtos;

public sealed record InventoryTransactionDto
{
    public Guid Id { get; init; }

    public Guid InventoryItemId { get; init; }

    public InventoryTransactionType TransactionType { get; init; }

    public int OnHandChanged { get; init; }

    public int ReservedChanged { get; init; }

    public int OnHandAfter { get; init; }

    public int ReservedAfter { get; init; }

    public Guid? ReferenceId { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}