using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly InventoryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(InventoryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(id , ct);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(List<T> list)
    {
        _dbSet.AddRangeAsync(list);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}