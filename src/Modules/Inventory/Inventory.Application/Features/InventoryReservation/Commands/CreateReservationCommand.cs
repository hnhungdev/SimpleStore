using Inventory.Domain.Entities;
using Inventory.Domain.Constants;
using Inventory.Application.Features.InventoryReservation.Dtos;
using Inventory.Domain.Enums;
using Inventory.Application.Abstractions;

namespace Inventory.Application.Features.InventoryReservation.Commands;

public sealed record CreateReservationCommand(CreateInventoryReservationDto Dto) : ICommand;

public sealed class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.Dto.OrderId)
            .NotEmpty()
            .WithMessage(InventoryErrorCode.OrderIdIsRequired);

        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(InventoryErrorCode.BadRequest);

        When(x => x.Dto != null, () =>
        {
            RuleFor(x => x.Dto.Items)
                .NotEmpty()
                .WithMessage(InventoryErrorCode.BadRequest);

            RuleForEach(x => x.Dto.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage(InventoryErrorCode.ProductIdIsRequired);

                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage(InventoryErrorCode.QuantityMustBeGreaterThanZero);
            });
        });
    }
}

public sealed class CreateReservationCommandHandler(
    IUnitOfWork unitOfWork,
    IInventoryOutboxStore outboxStore)
    : ICommandHandler<CreateReservationCommand>
{
    public async Task Handle(CreateReservationCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        var exists = await unitOfWork.InventoryReservations
            .ExistsByOrderIdAsync(dto.OrderId, ct);

        if (exists)
            throw new ClientValidationException(InventoryErrorCode.InventoryReservationAlreadyExists);

        var inventoryItems = await unitOfWork.InventoryItems
            .GetByProductIdsAsync(dto.Items.Select(x => x.ProductId).ToList(), ct);

        if (inventoryItems.Count != dto.Items.Count())
            throw new ClientValidationException(InventoryErrorCode.OneOrMoreInventoryItemsNotFound);

        var inventoryItemLookup = inventoryItems.ToDictionary(x => x.ProductId);

        foreach (var item in dto.Items)
        {
            var inventoryItem = inventoryItemLookup[item.ProductId];

            if (inventoryItem.Available < item.Quantity)
            {
                outboxStore.Add(
                    new InventoryReservationFailedIntegrationEvent
                    {
                        OrderId = dto.OrderId,
                        Reason = InventoryErrorCode.InsufficientStock
                    });

                return;
            }
        }

        foreach (var item in dto.Items)
        {
            var inventoryItem = inventoryItemLookup[item.ProductId];

            inventoryItem.Reserve(item.Quantity);

            var reservation = InventoryReservationEntity.Create(
                orderId: dto.OrderId,
                inventoryItemId: inventoryItem.Id,
                quantity: item.Quantity);

            var transaction = InventoryTransactionEntity.Create(
                inventoryItemId: inventoryItem.Id,
                transactionType: InventoryTransactionType.Reserve,
                onHandChanged: 0,
                reservedChanged: item.Quantity,
                onHandAfter: inventoryItem.OnHand,
                reservedAfter: inventoryItem.Reserved,
                referenceId: dto.OrderId);

            unitOfWork.InventoryReservations.Add(reservation);
            unitOfWork.InventoryTransactions.Add(transaction);
        }

        outboxStore.Add(
            new InventoryReservedIntegrationEvent
            {
                OrderId = dto.OrderId
            });

        await unitOfWork.SaveChangesAsync(ct);
    }
}