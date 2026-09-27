namespace BuildingBlocks.Domain.Abstractions;

public interface IAuditable
{
    string? CreatedBy { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }

    string? UpdatedBy { get; set; }
}
