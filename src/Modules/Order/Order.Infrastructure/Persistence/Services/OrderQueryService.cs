using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Order.Dtos;

namespace Order.Infrastructure.Persistence.Services;

public class OrderQueryService(OrderDbContext _context)
 : IOrderQueryService
{
    public async Task<OrderDto?> GetMyOrderByIdAsync(
        Guid orderId,
        Guid customerId,
        CancellationToken ct = default)
    {
        var result = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId && o.Customer.Id == customerId)
            .ToOrderDto()
            .FirstOrDefaultAsync(ct);

        return result;
    }

    public async Task<OrderDto?> GetOrderByIdAsync(
        Guid orderId,
        CancellationToken ct = default)
    {
        var result = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .ToOrderDto()
            .FirstOrDefaultAsync(ct);

        return result;
    }
    public async Task<PagedResult<OrderSummaryDto>> GetMyOrdersAsync(
        Guid customerId,
        GetMyOrdersFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Where(o => o.Customer.Id == customerId)
            .ApplyOrderFilter(filter);

        var result = await query
            .OrderByDescending(o => o.CreatedAt)
            .ToPagedResultAsync(OrderQueryExtensions.ToSummaryDto(), pagination, ct);

        return result;
    }

    public async Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(
        GetOrdersFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var query = _context.Orders
            .AsNoTracking()
            .ApplyOrderFilter(filter);

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(o =>
                o.Customer.Id == filter.CustomerId.Value);
        }

        var result = await query
            .OrderByDescending(o => o.CreatedAt)
            .ToPagedResultAsync(OrderQueryExtensions.ToSummaryDto(), pagination, ct);

        return result;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync(GetOrdersFilter filter, CancellationToken ct)
    {
        var query = _context.Orders
            .AsNoTracking()
            .ApplyOrderFilter(filter);

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(
                o => o.Customer.Id == filter.CustomerId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToOrderDto()
            .ToListAsync(ct);
    }
}
