using Order.Domain.Repositories;
using Order.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Order.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork

{
    private readonly OrderDbContext _context;
    private IDbContextTransaction? _transaction;

    public IOrderRepository Orders { get; }

    public UnitOfWork(OrderDbContext context
        , IOrderRepository orderRepository)
    {
        _context = context;
        Orders = orderRepository;
    }

   public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            if (_transaction != null)
                await _transaction.CommitAsync(ct);
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
