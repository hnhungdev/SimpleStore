using Cart.Application.Abstractions;

namespace Cart.Application.Features.Cart.Commands;

public sealed record ClearCartCommand(Guid UserId) : ICommand;

public class ClearCartCommandHandler(IUnitOfWork unitOfWork,
    ICartCache cartCache)
    : ICommandHandler<ClearCartCommand>
{
    public async Task Handle(ClearCartCommand command, CancellationToken ct)
    {
        var existingCart = await unitOfWork.Cart.GetByUserAsync(command.UserId, ct);

        existingCart.Clear();

        await unitOfWork.SaveChangesAsync(ct);

        await cartCache.RemoveAsync(command.UserId, ct);
    }
}