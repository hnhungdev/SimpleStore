using Inventory.Application.Abstractions;
using Inventory.Domain.Constants;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.InventoryReservation.Commands;

public sealed record CommitReservationCommand(Guid OrderId) : ICommand;

public sealed class CommitReservationCommandValidator : AbstractValidator<CommitReservationCommand>
{
    public CommitReservationCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(InventoryErrorCode.OrderIdIsRequired);
    }
}

public sealed class CommitReservationCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CommitReservationCommand>
{
    public async Task Handle(CommitReservationCommand command, CancellationToken ct)
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

            inventoryItem.Commit(reservation.Quantity);
            
            reservation.Commit();

            var transaction = InventoryTransactionEntity.Create(
                inventoryItemId: inventoryItem.Id,
                transactionType: InventoryTransactionType.Commit,
                onHandChanged: -reservation.Quantity,
                reservedChanged: -reservation.Quantity,
                onHandAfter: inventoryItem.OnHand,
                reservedAfter: inventoryItem.Reserved,
                referenceId: command.OrderId);

            unitOfWork.InventoryTransactions.Add(transaction);
        }

        await unitOfWork.SaveChangesAsync(ct);
    }
}

