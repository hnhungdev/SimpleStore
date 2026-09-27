namespace BuildingBlocks.Domain.Exceptions;

public sealed class DomainException : Exception
{
    public object? Details { get; }

    public DomainException(string message, object? details = null) : base(message)
    {
        Details = details;
    }
}