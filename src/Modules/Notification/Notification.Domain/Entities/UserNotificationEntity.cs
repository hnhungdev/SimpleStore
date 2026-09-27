using Notification.Domain.Constants;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities;

public sealed class UserNotificationEntity : AuditableEntity
{
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = default!;

    public string Message { get; private set; } = default!;

    public NotificationType NotificationType { get; private set; }

    public bool IsRead { get; private set; }

    public DateTimeOffset? ReadAt { get; private set; }

    public string? TargetUrl { get; private set; }

    public static UserNotificationEntity Create(
        Guid userId,
        string title,
        string message,
        NotificationType notificationType,
        string? targetUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(NotificationErrorCode.TitleIsRequired);

        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException(NotificationErrorCode.MessageIsRequired);

        return new UserNotificationEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            NotificationType = notificationType,
            TargetUrl = targetUrl,
            IsRead = false,
        };
    }

    public void MarkAsRead()
    {
        if (IsRead) 
            return;

        IsRead = true;
        ReadAt = DateTimeOffset.UtcNow;
    }
}