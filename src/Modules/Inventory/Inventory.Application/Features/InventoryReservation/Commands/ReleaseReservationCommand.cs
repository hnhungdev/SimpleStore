using Inventory.Application.Abstractions;
using Inventory.Domain.Constants;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryReservation.Commands;

public sealed record ReleaseReservationCommand(Guid OrderId) : ICommand;

public sealed class ReleaseReservationCommandValidator : AbstractValidator<ReleaseReservationCommand>
{
    public ReleaseReservationCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(InventoryErrorCode.OrderIdIsRequired);
    }
}

public sealed class ReleaseReservationCommandHandler(
    IUnitOfWork unitOfWork,
    IInventoryOutboxStore outboxStore)
    : ICommandHandler<ReleaseReservationCommand>
{
    public async Task Handle(ReleaseReservationCommand command, CancellationToken ct)
    {
        var reservations = await unitOfWork.InventoryReservations
           .GetByOrderIdAsync(command.OrderId, ct);

        if (reservations.Count == 0)
            throw new NotFoundException(InventoryErrorCode.InventoryReservationNotFound);

        var inventoryItems = await unitOfWork.InventoryItems
            .GetByIdsAsync(reservations.Select(x => x.InventoryItemId).ToList(), ct);

        if (inventoryItems.Count != reservations.Count)
            throw new ClientValidationException(InventoryErrorCode.OneOrMoreInventoryItemsNotFound);

        var lookup = inventoryItems.ToDictionary(x => x.Id);

        foreach (var reservation in reservations)
        {
            var inventoryItem = lookup[reservation.InventoryItemId];

            inventoryItem.Release(reservation.Quantity);

            reservation.Release();

            var transaction = InventoryTransactionEntity.Create(
                inventoryItemId: inventoryItem.Id,
                transactionType: InventoryTransactionType.Release,
                onHandChanged: 0,
                reservedChanged: -reservation.Quantity,
                onHandAfter: inventoryItem.OnHand,
                reservedAfter: inventoryItem.Reserved,
                referenceId: command.OrderId);

            unitOfWork.InventoryTransactions.Add(transaction);
        }

        outboxStore.Add(
            new InventoryReleasedIntegrationEvent
            {
                OrderId = command.OrderId
            });

        await unitOfWork.SaveChangesAsync(ct);
    }
}

