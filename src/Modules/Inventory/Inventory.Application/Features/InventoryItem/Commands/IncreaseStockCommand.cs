using Inventory.Application.Abstractions;
using Inventory.Domain.Constants;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryItem.Commands;

public sealed record IncreaseStockCommand(Guid InventoryItemId, int Quantity) : ICommand;

public sealed class IncreaseStockCommandValidator : AbstractValidator<IncreaseStockCommand>
{
    public IncreaseStockCommandValidator()
    {
        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage(InventoryErrorCode.InventoryItemIdIsRequired);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(InventoryErrorCode.QuantityMustBeGreaterThanZero);
    }
}

public sealed class IncreaseStockCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<IncreaseStockCommand>
{
    public async Task Handle(IncreaseStockCommand command, CancellationToken ct)
    {
        var entity = await unitOfWork.InventoryItems
            .GetByIdAsync(command.InventoryItemId, ct)
            ?? throw new NotFoundException(InventoryErrorCode.InventoryItemNotFound);

        var previousOnHand = entity.OnHand;
        var newQuantity = previousOnHand + command.Quantity;
        var diff = newQuantity - previousOnHand;

        if (diff == 0)
            return;

        entity.Increase(command.Quantity);

        var transaction = InventoryTransactionEntity.Create(
            inventoryItemId: entity.Id,
            transactionType: InventoryTransactionType.Increase,
            onHandChanged: command.Quantity,
            reservedChanged: 0,
            onHandAfter: entity.OnHand,
            reservedAfter: entity.Reserved,
            referenceId: null);

        unitOfWork.InventoryTransactions.Add(transaction);

        await unitOfWork.SaveChangesAsync(ct);
    }
}