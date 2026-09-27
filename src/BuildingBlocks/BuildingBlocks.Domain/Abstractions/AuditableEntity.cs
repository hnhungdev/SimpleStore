namespace BuildingBlocks.Domain.Abstractions;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
}