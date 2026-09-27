using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class EntityEntryExtensions
{
    /// <summary>
    /// Checks whether any EF Core complex property has been modified.
    /// </summary>
    public static bool HasChangedComplexProperties(
        this EntityEntry entry)
    {
        return entry.ComplexProperties
            .Any(cp => cp.IsModified);
    }

    /// <summary>
    /// Checks whether any EF Core owned reference (OwnsOne)
    /// has been added, modified, or deleted.
    /// </summary>
    public static bool HasChangedOwnedReferences(
        this EntityEntry entry)
    {
        return entry.References.Any(reference =>
            reference.TargetEntry != null &&
            reference.TargetEntry.Metadata.IsOwned() &&
            reference.TargetEntry.State is
                EntityState.Added or
                EntityState.Modified or
                EntityState.Deleted);
    }

    /// <summary>
    /// Checks whether any EF Core owned collection (OwnsMany)
    /// contains an added, modified, or deleted entity.
    /// </summary>
    public static bool HasChangedOwnedCollections(
        this EntityEntry entry)
    {
        return entry.Collections.Any(collection =>
            collection.Metadata.TargetEntityType.IsOwned() &&
            collection.CurrentValue != null &&
            collection.CurrentValue
                .Cast<object>()
                .Any(item =>
                {
                    var itemEntry = entry.Context.Entry(item);

                    return itemEntry.State is
                        EntityState.Added or
                        EntityState.Modified or
                        EntityState.Deleted;
                }));
    }

    /// <summary>
    /// Checks whether any owned entity or complex property
    /// related to the entity has changed.
    /// </summary>
    public static bool HasChangedOwnedOrComplexProperties(
        this EntityEntry entry)
    {
        return entry.HasChangedComplexProperties()
            || entry.HasChangedOwnedReferences()
            || entry.HasChangedOwnedCollections();
    }
}