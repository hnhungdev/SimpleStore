namespace Notification.Domain.Enums;

public enum NotificationJobStatus
{
    Queued = 1,
    Retrying = 2,
    Sent = 3,
    Failed = 4,
}
