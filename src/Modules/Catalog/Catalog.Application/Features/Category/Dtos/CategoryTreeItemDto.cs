namespace Catalog.Application.Features.Category.Dtos;

public sealed record CategoryTreeDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public List<CategoryTreeDto> Children { get; init; } = [];
}
