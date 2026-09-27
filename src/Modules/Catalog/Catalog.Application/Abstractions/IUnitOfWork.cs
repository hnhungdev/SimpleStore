using Catalog.Domain.Repositories;

namespace Catalog.Application.Abstractions;

public interface IUnitOfWork
{
    IProductRepository Products { get; }

    ICategoryRepository Categories{ get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}