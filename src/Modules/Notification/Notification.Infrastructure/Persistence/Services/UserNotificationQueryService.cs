using Microsoft.EntityFrameworkCore;
using Notification.Application.Features.Notification.Dtos;
using Notification.Application.Features.Notification.Services;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence.Services;

public class UserNotificationQueryService : IUserNotificationQueryService
{
    private readonly NotificationDbContext _context;
    private readonly DbSet<UserNotificationEntity> _dbSet;

    public UserNotificationQueryService(NotificationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<UserNotificationEntity>();
    }

    public async Task<int> GetCountNotificationUnreadAsync(Guid userId, CancellationToken ct = default)
    {
        return await _dbSet.CountAsync(x => x.IsRead == false);
    }

    public async Task<PagedResult<NotificationDto>> GetNotificationsAsync(Guid userId, NotificationFilter filter, PaginationRequest pagination, CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking();
        if (filter.IsRead.HasValue)
        {
            query = query.Where(x => x.IsRead == filter.IsRead.Value);
        }

        return await query
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToPagedResultAsync(
                x => new NotificationDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Title = x.Title,
                    Message = x.Message,
                    NotificationType = x.NotificationType,
                    IsRead = x.IsRead,
                    ReadAt = x.ReadAt,
                    CreatedAt = x.CreatedAt
                },
                pagination, ct);
    }

    public async Task<RecentNotificationsDto> GetRecentNotificationsAsync(Guid userId, CancellationToken ct = default)
    {
        var unreadCount = await GetCountNotificationUnreadAsync(userId, ct);

        var items = await _dbSet.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .Select(x => new NotificationDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Message = x.Message,
                NotificationType = x.NotificationType,
                IsRead = x.IsRead,
                ReadAt = x.ReadAt,
                CreatedAt = x.CreatedAt
            }).ToListAsync();

        return new RecentNotificationsDto
        {
            Items = items,
            UnreadCount = unreadCount
        };
    }
}