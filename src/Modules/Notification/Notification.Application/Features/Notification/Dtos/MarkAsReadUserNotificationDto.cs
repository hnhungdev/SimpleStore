namespace Notification.Application.Features.Notification.Dtos;

public sealed class MarkAsReadNotificationDto
{
    public List<Guid> Ids { get; set; } = default!;
}
