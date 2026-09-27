namespace Catalog.Application.Features.Category.Dtos;

public sealed record CategoryDto 
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Slug { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public string? ParentName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
