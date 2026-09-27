using Cart.Domain.Constants;
using Cart.Application.Features.Cart.Dtos;
using Cart.Application.Abstractions;
using Cart.Domain.ValueObjects;

namespace Cart.Application.Features.Cart.Commands;

public sealed record AddCartItemCommand(Guid UserId, Guid ProductId , AddCartItemDto Dto) : ICommand;

public sealed class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(CartErrorCode.UserIdIsRequired);

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CartErrorCode.ProductIdIsRequired);

        RuleFor(x => x.Dto.Quantity)
            .GreaterThan(0)
            .WithMessage(CartErrorCode.QuantityMustBeGreaterThanZero);
    }
}

public sealed class AddCartItemCommandHandler(
    IUnitOfWork unitOfWork,
    ICatalogModule catalogModule,
    ICartCache cartCache)
    : ICommandHandler<AddCartItemCommand>
{
    public async Task Handle(AddCartItemCommand command, CancellationToken ct)
    {

        var cart = await unitOfWork.Cart.GetByUserAsync(command.UserId, ct);

        var product = await catalogModule.GetOrderableProductAsync(command.ProductId, ct);

        if (product is null)
            throw new NotFoundException(CartErrorCode.ProductNotFound, command.ProductId);

        cart.AddItem(
            Product.Of(command.ProductId, product.Name, product.ThumbnailUrl, product.Price),
            command.Dto.Quantity);

        await unitOfWork.SaveChangesAsync(ct);

        await cartCache.RemoveAsync(command.UserId, ct);
    }
}