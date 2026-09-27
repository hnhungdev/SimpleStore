namespace BuildingBlocks.Application.Pagination;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    private PagedResult(
        IReadOnlyList<T> items,
        int pageIndex,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = pageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int pageIndex,
        int pageSize,
        int totalCount)
    {
        if (pageIndex <= 0)
            throw new ArgumentException("PageIndex must be >= 1");

        if (pageSize <= 0)
            throw new ArgumentException("PageSize must be >= 1");

        var list = items as IReadOnlyList<T> ?? items.ToList();

        return new PagedResult<T>(
            list,
            pageIndex,
            pageSize,
            totalCount);
    }
}