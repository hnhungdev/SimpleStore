using Catalog.Application.Abstractions;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Commands;

public record DeleteProductCommand(Guid ProductId) : ICommand;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.ProductIdIsRequired);
    }
}

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await unitOfWork.Products.GetByIdAsync(command.ProductId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound, command.ProductId);

        unitOfWork.Products.Remove(product);

        await unitOfWork.SaveChangesAsync(ct);
    }
}