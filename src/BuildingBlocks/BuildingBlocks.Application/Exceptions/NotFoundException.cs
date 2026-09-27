namespace BuildingBlocks.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public object? Details { get; }

    public NotFoundException(string message, object? details = null) : base(message)
    {
        Details = details;
    }
}
