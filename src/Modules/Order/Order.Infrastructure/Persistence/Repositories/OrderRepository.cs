using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Domain.Repositories;

namespace Order.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;
    private readonly DbSet<OrderEntity> _dbSet;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
        _dbSet = context.Set<OrderEntity>();
    }

    public void Add(OrderEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task<OrderEntity?> GetByOrderIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet
            .FindAsync([id], ct);
    }

    public async Task<OrderEntity?> GetByOrderIdAndUserIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id && x.Customer.Id == userId, ct);
    }
}
