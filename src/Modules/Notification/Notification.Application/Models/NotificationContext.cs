namespace Notification.Application.Models;

public sealed class NotificationContext
{
    public Guid UserId { get; init; }

    public string Recipient { get; init; } = default!;

    public string Subject { get; init; } = default!;

    public string? Body { get; init; } = default!;

    public bool IsHtml { get; init; }

    public string? TargetUrl { get; set; }
}