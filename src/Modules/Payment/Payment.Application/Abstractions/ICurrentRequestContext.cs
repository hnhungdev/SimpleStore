namespace Payment.Application.Abstractions;

public interface ICurrentRequestContext
{
    string IpAddress { get; }
}