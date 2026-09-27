using Inventory.Domain.Constants;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Application.Features.InventoryItem.Dtos;
using Inventory.Application.Abstractions;

namespace Inventory.Application.Features.InventoryItem.Commands;

public sealed record CreateInventoryItemCommand(CreateInventoryItemDto Dto) : ICommand<Guid>;

public sealed class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(InventoryErrorCode.BadRequest);

        When(x => x.Dto != null, () =>
        {
            RuleFor(x => x.Dto.ProductId)
                .NotEmpty()
                .WithMessage(InventoryErrorCode.ProductIdIsRequired);

            RuleFor(x => x.Dto.Quantity)
                .GreaterThan(0)
                .WithMessage(InventoryErrorCode.QuantityMustBeGreaterThanZero);
        });
    }
}

public sealed class CreateInventoryItemCommandHandler(
    IUnitOfWork unitOfWork,
    ICatalogModule catalogModule) : ICommandHandler<CreateInventoryItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateInventoryItemCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        var product = await catalogModule.GetProductByIdAsync(dto.ProductId, ct)
            ?? throw new NotFoundException(InventoryErrorCode.ProductNotFound, dto.ProductId);

        var existingInventoryItem = await unitOfWork.InventoryItems
            .GetByProductIdAsync(dto.ProductId, ct);

        if (existingInventoryItem is not null)
            throw new ClientValidationException(InventoryErrorCode.InventoryItemAlreadyExists);

        var entity = InventoryItemEntity.Create(
            product.Id,
            product.Name,
            dto.Quantity);

        unitOfWork.InventoryItems.Add(entity);

        var transaction = InventoryTransactionEntity.Create(
            inventoryItemId: entity.Id,
            transactionType: InventoryTransactionType.Increase,
            onHandChanged: entity.OnHand,
            reservedChanged: 0,
            onHandAfter: entity.OnHand,
            reservedAfter: 0,
            referenceId: null);

        unitOfWork.InventoryTransactions.Add(transaction);

        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }
}