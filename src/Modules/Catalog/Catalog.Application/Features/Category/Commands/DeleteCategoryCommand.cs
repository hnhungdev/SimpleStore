using Catalog.Application.Abstractions;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Category.Commands;

public record DeleteCategoryCommand(Guid CategoryId) : ICommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.CategoryIdIsRequired);
    }
}

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand command, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(command.CategoryId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.CategoryNotFound, command.CategoryId);
        
        await unitOfWork.Categories.MoveChildrenAsync(command.CategoryId, category.ParentId, ct);

        unitOfWork.Categories.Remove(category);
        
        await unitOfWork.SaveChangesAsync(ct);
    }
}
