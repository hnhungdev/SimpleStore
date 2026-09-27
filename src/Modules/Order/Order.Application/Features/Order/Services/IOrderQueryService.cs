using Order.Application.Features.Order.Dtos;

public interface IOrderQueryService
{
    Task<OrderDto?> GetMyOrderByIdAsync(Guid orderId, Guid customerId, CancellationToken ct = default);
    
    Task<PagedResult<OrderSummaryDto>> GetMyOrdersAsync(Guid customerId, GetMyOrdersFilter filter, PaginationRequest pagination, CancellationToken ct = default);

    Task<OrderDto?> GetOrderByIdAsync(Guid orderId, CancellationToken ct = default);
 
    Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(GetOrdersFilter filter, PaginationRequest pagination, CancellationToken ct = default);

    Task<List<OrderDto>> GetAllOrdersAsync(GetOrdersFilter filter, CancellationToken ct); 
}