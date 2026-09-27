using Notification.Domain.Enums;

namespace Notification.Application.Features.Notification.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public string? Title { get; set; }

    public string? Message { get; set; }

    public NotificationType NotificationType { get; set; }

    public bool IsRead { get; set; }

    public DateTimeOffset? ReadAt { get; set; }

    public string? TargetUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
