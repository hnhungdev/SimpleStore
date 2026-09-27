using Catalog.Application.Abstractions;
using Catalog.Application.Features.Product.Dtos;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Commands;

public record ChangeProductStatusCommand(Guid ProductId, ChangeProductStatusDto Dto) : ICommand;

public class ChangeProductStatusCommandValidator : AbstractValidator<ChangeProductStatusCommand>
{
    public ChangeProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.ProductIdIsRequired);

        RuleFor(x => x.Dto.Status)
            .IsInEnum()
            .WithMessage(CatalogErrorCode.StatusIsInvalid);
    }
}

public class ChangeProductStatusCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<ChangeProductStatusCommand>
{
    public async Task Handle(ChangeProductStatusCommand command, CancellationToken ct)
    {

        var entity = await unitOfWork.Products.GetByIdAsync(command.ProductId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound, command.ProductId);

        entity.ChangeStatus(command.Dto.Status);

        await unitOfWork.SaveChangesAsync(ct);
    }
}