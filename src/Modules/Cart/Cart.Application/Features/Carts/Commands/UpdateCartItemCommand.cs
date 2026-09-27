using Cart.Domain.Constants;
using Cart.Application.Features.Cart.Dtos;
using Cart.Application.Abstractions;

namespace Cart.Application.Features.Cart.Commands;

public sealed record UpdateCartItemCommand(Guid UserId, Guid ProductId, UpdateQuantityDto Dto) : ICommand;

public sealed class UpdateCartItemValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CartErrorCode.ProductIdIsRequired);

        RuleFor(x => x.Dto.Quantity)
            .GreaterThan(0)
            .WithMessage(CartErrorCode.QuantityMustBeGreaterThanZero);
    }
}

public sealed class UpdateCartItemHandler(
    IUnitOfWork unitOfWork,
    ICartCache cartCache,
    ICatalogModule catalogModule)
    : ICommandHandler<UpdateCartItemCommand>
{
    public async Task Handle(
        UpdateCartItemCommand command,
        CancellationToken ct)
    {
        var cart = await unitOfWork.Cart
            .GetByUserAsync(command.UserId, ct);

        var product = await catalogModule.GetOrderableProductAsync(command.ProductId, ct);

        if (product is null)
            throw new NotFoundException(CartErrorCode.ProductNotFound, command.ProductId);

        cart.SetQuantity(
            command.ProductId,
            command.Dto.Quantity);

        await unitOfWork.SaveChangesAsync(ct);

        await cartCache.RemoveAsync(command.UserId, ct);
    }
}