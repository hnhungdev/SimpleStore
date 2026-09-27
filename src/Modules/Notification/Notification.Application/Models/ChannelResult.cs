namespace Notification.Application.Models;

public sealed class ChannelResult
{
    public bool IsSuccess { get; }

    public string? ProviderMessageId { get; }

    public string? ErrorMessage { get; }

    private ChannelResult(bool ok, string? id = null, string? msg = null)
    {
        IsSuccess = ok; ProviderMessageId = id; ErrorMessage = msg;
    }


    public static ChannelResult Success(string? providerMessageId = null) =>
        new(true, providerMessageId);

    public static ChannelResult Failure(string errorMessage) =>
        new(false, null, errorMessage);

}