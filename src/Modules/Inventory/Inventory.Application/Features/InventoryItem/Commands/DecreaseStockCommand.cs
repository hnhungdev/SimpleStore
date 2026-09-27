using Inventory.Application.Abstractions;
using Inventory.Domain.Constants;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryItem.Commands;

public sealed record DecreaseStockCommand(Guid InventoryItemId, int Quantity) : ICommand;

public sealed class DecreaseStockCommandValidator : AbstractValidator<DecreaseStockCommand>
{
    public DecreaseStockCommandValidator()
    {
        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage(InventoryErrorCode.InventoryItemIdIsRequired);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(InventoryErrorCode.QuantityMustBeGreaterThanZero);
    }
}

public sealed class DecreaseStockCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DecreaseStockCommand>
{
    public async Task Handle(DecreaseStockCommand command, CancellationToken ct)
    {
        var entity = await unitOfWork.InventoryItems
            .GetByIdAsync(command.InventoryItemId, ct)
            ?? throw new NotFoundException(InventoryErrorCode.InventoryItemNotFound);

        var previousOnHand = entity.OnHand;
        var newQuantity = previousOnHand - command.Quantity;
        var diff = newQuantity - previousOnHand;

        if (diff == 0)
            return;

        entity.Decrease(command.Quantity);

        var transaction = InventoryTransactionEntity.Create(
            inventoryItemId: entity.Id,
            transactionType: InventoryTransactionType.Decrease,
            onHandChanged: -command.Quantity,
            reservedChanged: 0,
            onHandAfter: entity.OnHand,
            reservedAfter: entity.Reserved,
            referenceId: null);

        unitOfWork.InventoryTransactions.Add(transaction);

        await unitOfWork.SaveChangesAsync(ct);
    }   
}