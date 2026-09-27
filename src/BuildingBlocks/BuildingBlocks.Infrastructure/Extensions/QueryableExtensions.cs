using BuildingBlocks.Application.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));
        if (pagination is null) throw new ArgumentNullException(nameof(pagination));

        var pageIndex = pagination.PageIndex <= 0 ? 1 : pagination.PageIndex;
        var pageSize = pagination.PageSize <= 0 ? 10 : pagination.PageSize;

        var totalCount = await query.CountAsync(ct);

        if (totalCount == 0)
        {
            return PagedResult<T>.Create(
                items: [],
                pageIndex: pageIndex,
                pageSize: pageSize,
                totalCount: 0);
        }

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(
            items: items,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TResult>> selector,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (pagination is null) throw new ArgumentNullException(nameof(pagination));

        var pageIndex = pagination.PageIndex <= 0 ? 1 : pagination.PageIndex;
        var pageSize = pagination.PageSize <= 0 ? 10 : pagination.PageSize;

        var totalCount = await query.CountAsync(ct);

        if (totalCount == 0)
        {
            return PagedResult<TResult>.Create(
                items: [],
                pageIndex: pageIndex,
                pageSize: pageSize,
                totalCount: 0);
        }

        var items = await query 
            .Select(selector)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return PagedResult<TResult>.Create(
            items: items,
            pageIndex: pageIndex,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string? sortBy,
            bool ascending = true)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression property = parameter;

        foreach (var member in sortBy.Split('.'))
        {
            property = Expression.PropertyOrField(property, member);
        }

        var lambda = Expression.Lambda(property, parameter);

        var methodName = ascending
            ? nameof(Queryable.OrderBy)
            : nameof(Queryable.OrderByDescending);

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExpression);
    }
}