namespace BuildingBlocks.Application.Pagination;

public sealed record PaginationRequest(int PageIndex = 1, int PageSize = 10);
