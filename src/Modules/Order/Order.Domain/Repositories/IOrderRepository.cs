using Order.Domain.Entities;

namespace Order.Domain.Repositories;

public interface IOrderRepository 
{
    void Add(OrderEntity entity);

    Task<OrderEntity?> GetByOrderIdAsync(Guid id, CancellationToken ct = default);

    Task<OrderEntity?> GetByOrderIdAndUserIdAsync(Guid id, Guid userId, CancellationToken ct = default);
}
