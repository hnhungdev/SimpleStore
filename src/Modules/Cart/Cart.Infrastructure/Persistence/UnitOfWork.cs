using Cart.Domain.Repositories;
using Cart.Application.Abstractions;

namespace Cart.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CartDbContext _context;
    public ICartRepository Cart { get; }

    public UnitOfWork(CartDbContext context
        , ICartRepository cartRepository)
    {
        _context = context;
        Cart = cartRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}