namespace Catalog.Application.Features.Category.Dtos;

public sealed record CreateCategoryDto(string Name, string Slug, string? Description, Guid? ParentId);