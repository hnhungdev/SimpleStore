using Catalog.Domain.Constants;

namespace Catalog.Domain.Entities;

public sealed class CategoryEntity : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string Slug { get; private set; } = string.Empty;

    public Guid? ParentId { get; private set; }

    public CategoryEntity? Parent { get; private set; }

    private readonly List<CategoryEntity> _children = new();

    public IReadOnlyList<CategoryEntity> Children
        => _children.AsReadOnly();

    private CategoryEntity() { }

    public static CategoryEntity Create(
        string name,
        string slug,
        string? description,
        Guid? parentId = null)
    {
        ValidateCategory(name, slug);

        return new CategoryEntity
        {
            Name = name,
            Description = description,
            Slug = slug,
            ParentId = parentId,
        };
    }

    public void Update(
        string name,
        string slug,
        string? description,
        Guid? parentId)
    {
        ValidateCategory(name, slug);

        ChangeParent(parentId);

        Name = name;
        Slug = slug;
        Description = description;
    }

    public void ChangeParent(Guid? parentId)
    {
        if (parentId == Id)
            throw new DomainException(CatalogErrorCode.CategoryCannotBeItsOwnParent);

        ParentId = parentId;
    }

    private static void ValidateCategory(string name, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(CatalogErrorCode.CategoryNameIsRequired);

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException(CatalogErrorCode.SlugIsRequired);
    }
}