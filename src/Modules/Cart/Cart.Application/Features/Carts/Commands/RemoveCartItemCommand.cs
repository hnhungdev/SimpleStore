using Cart.Domain.Constants;
using Cart.Application.Abstractions;

namespace Cart.Application.Features.Cart.Commands;

public sealed record RemoveCartItemCommand(Guid UserId, Guid ProductId) : ICommand;

public sealed class RemoveCartItemValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CartErrorCode.ProductIdIsRequired);
    }
}

public sealed class RemoveCartItemCommandHandler(
    IUnitOfWork unitOfWork,
    ICartCache cartCache)
    : ICommandHandler<RemoveCartItemCommand>
{

    public async Task Handle(
        RemoveCartItemCommand command,
        CancellationToken ct)
    {
        var cart = await unitOfWork.Cart.GetByUserAsync(command.UserId, ct);

        cart.RemoveItem(command.ProductId);

        await unitOfWork.SaveChangesAsync(ct);

        await cartCache.RemoveAsync(command.UserId, ct);
    }
}