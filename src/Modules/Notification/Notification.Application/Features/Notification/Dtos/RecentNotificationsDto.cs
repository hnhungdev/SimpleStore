namespace Notification.Application.Features.Notification.Dtos;

public class RecentNotificationsDto
{
    public List<NotificationDto> Items { get; set; }= new();

    public int UnreadCount { get; init; }
}