namespace Catalog.Application.Features.Category.Dtos;

public sealed record UpdateCategoryDto(string Name, string Slug, string? Description, Guid? ParentId);