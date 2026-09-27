using Cart.Domain.Repositories;

namespace Cart.Application.Abstractions;

public interface IUnitOfWork
{
    ICartRepository Cart { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}