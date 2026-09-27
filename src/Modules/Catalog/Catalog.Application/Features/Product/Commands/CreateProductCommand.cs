using Catalog.Domain.Constants;
using Catalog.Application.Features.Product.Dtos;
using Catalog.Domain.Entities;
using Catalog.Application.Abstractions;

namespace Catalog.Application.Features.Product.Commands;

public record CreateProductCommand(CreateProductDto Dto) : ICommand<Guid>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.ProductNameIsRequired);

        RuleFor(x => x.Dto.Price)
            .GreaterThan(0)
            .WithMessage(CatalogErrorCode.PriceMustBeGreaterThanZero);

        RuleFor(x => x.Dto.ThumbnailUrl)
            .Empty()
            .WithMessage(CatalogErrorCode.ThumbnailUrlIsRequired)
            .MustBeAbsoluteUrl()
            .WithMessage(CatalogErrorCode.ThumbnailUrlIsInvalid);

        RuleFor(x => x.Dto.Slug)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.SlugIsRequired)
            .IsValidSlug()
            .WithMessage(CatalogErrorCode.SlugIsInvalid);

        RuleForEach(x => x.Dto.Images)
            .MustBeAbsoluteUrl()
            .WithMessage(CatalogErrorCode.ImageGalleryUrlIsInvalid);
    }
}

public class CreateProductCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        await EnsureCategoriesExistAsync(dto.CategoryIds, ct);

        if (await unitOfWork.Products.IsSlugTakenAsync(dto.Slug, exceptId: null, ct))
            throw new ClientValidationException(CatalogErrorCode.SlugIsAlreadyTaken);

        var product = ProductEntity.Create(
            name: dto.Name,
            slug: dto.Slug,
            description: dto.Description,
            price: dto.Price,
            thumbnailUrl: dto.ThumbnailUrl,
            galleryImageUrls: dto.Images,
            categoryIds: dto.CategoryIds);

        unitOfWork.Products.Add(product);

        await unitOfWork.SaveChangesAsync(ct);

        return product.Id;
    }

    private async Task EnsureCategoriesExistAsync(
        IReadOnlyCollection<Guid> categoryIds,
        CancellationToken ct)
    {
        if (categoryIds.Count == 0)
            return;

        var distinctIds = categoryIds
            .Distinct()
            .ToList();

        var existingIds = await unitOfWork.Categories
            .GetExistingIdsAsync(distinctIds, ct);

        var existingSet = existingIds.ToHashSet();

        var invalidIds = distinctIds
            .Where(id => !existingSet.Contains(id))
            .ToList();

        if (invalidIds.Count > 0)
        {
            throw new NotFoundException(CatalogErrorCode.CategoriesNotFound, string.Join(", ", invalidIds));
        }
    }
}
