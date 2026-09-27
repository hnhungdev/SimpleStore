using Catalog.Domain.Constants;
using Catalog.Application.Features.Category.Dtos;
using Catalog.Domain.Entities;
using Catalog.Application.Abstractions;

namespace Catalog.Application.Features.Category.Commands;

public record CreateCategoryCommand(CreateCategoryDto Dto) : ICommand<Guid>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
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

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        if (dto.ParentId.HasValue)
        {
            _ = await unitOfWork.Categories.GetByIdAsync(dto.ParentId.Value, ct)
                ?? throw new NotFoundException(CatalogErrorCode.CategoryNotFound);
        }

        if (await unitOfWork.Categories.IsSlugTakenAsync(dto.Slug, null, ct))
            throw new ClientValidationException(CatalogErrorCode.SlugIsAlreadyTaken);

        var category = CategoryEntity.Create(
            name: dto.Name,
            slug: dto.Slug,
            description: dto.Description,
            parentId: dto.ParentId);

        unitOfWork.Categories.Add(category);

        await unitOfWork.SaveChangesAsync(ct);

        return category.Id;
    }
}
