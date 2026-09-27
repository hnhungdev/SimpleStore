using Catalog.Application.Abstractions;
using Catalog.Application.Features.Product.Dtos;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Commands;

public record UpdateProductCommand(Guid ProductId, UpdateProductDto Dto) : ICommand;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(CatalogErrorCode.ProductIdIsRequired);

        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(CatalogErrorCode.BadRequest);

        When(x => x.Dto != null, () =>
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

            RuleForEach(x => x.Dto.Images)
                .MustBeAbsoluteUrl()
                .WithMessage(CatalogErrorCode.ImageGalleryUrlIsInvalid);
        });
    }
}

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        await EnsureCategoriesExistAsync(dto.CategoryIds, ct);

        var product = await unitOfWork.Products.GetByIdWithCategoriesAsync(command.ProductId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound);

        if (await unitOfWork.Products.IsSlugTakenAsync(dto.Slug, product.Id, ct))
            throw new ClientValidationException(CatalogErrorCode.SlugIsAlreadyTaken);

        product.Update(
            name: dto.Name,
            slug: dto.Slug,
            description: dto.Description,
            price: dto.Price,
            thumbnailUrl: dto.ThumbnailUrl,
            galleryImageUrls: dto.Images,
            categoryIds: dto.CategoryIds);

        await unitOfWork.SaveChangesAsync(ct);
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