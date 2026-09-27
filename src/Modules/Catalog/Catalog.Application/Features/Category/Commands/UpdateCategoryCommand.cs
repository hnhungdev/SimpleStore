using Catalog.Domain.Constants;
using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Abstractions;

namespace Catalog.Application.Features.Category.Commands;

public record UpdateCategoryCommand(Guid CategoryId, UpdateCategoryDto Dto) : ICommand;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.CategoryIdIsRequired);

        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(CatalogErrorCode.BadRequest);

        When(x => x.Dto != null, () =>
        {
            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .WithMessage(CatalogErrorCode.CategoryNameIsRequired);

            RuleFor(x => x.Dto.Slug)
                .NotEmpty()
                .WithMessage(CatalogErrorCode.SlugIsRequired)
                .IsValidSlug()
                .WithMessage(CatalogErrorCode.SlugIsInvalid);
        });
    }
}

public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        var category = await unitOfWork.Categories.GetByIdAsync(command.CategoryId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.CategoryNotFound, command.CategoryId);

        if (dto.ParentId.HasValue)
        {
            _ = await unitOfWork.Categories.GetByIdAsync(dto.ParentId.Value, ct)
                ?? throw new NotFoundException(CatalogErrorCode.CategoryNotFound);

            var isDescendant = await unitOfWork.Categories
                .IsDescendantOfAsync(dto.ParentId.Value, command.CategoryId, ct);

            if (isDescendant)
                throw new ClientValidationException(CatalogErrorCode.CategoryParentCycleDetected);
        }

        if (await unitOfWork.Categories.IsSlugTakenAsync(dto.Slug, category.Id, ct))
            throw new ClientValidationException(CatalogErrorCode.SlugIsAlreadyTaken);

        category.Update(
            name: dto.Name,
            slug: dto.Slug,
            description: dto.Description,
            parentId: dto.ParentId);

        await unitOfWork.SaveChangesAsync(ct);
    }
}
