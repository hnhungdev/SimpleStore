namespace BuildingBlocks.Application.Exceptions;

public class ClientValidationException : Exception
{
    public object? Details { get; set; }

    public ClientValidationException(string message, object? details = null) : base(message)
    {
        Details = details;
    }
}