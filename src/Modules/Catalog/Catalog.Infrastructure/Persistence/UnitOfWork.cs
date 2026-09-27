using Catalog.Application.Abstractions;
using Catalog.Domain.Repositories;

namespace Catalog.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CatalogDbContext _context;
    public ICategoryRepository Categories { get; }
    public IProductRepository Products { get; }
    public UnitOfWork(CatalogDbContext context
        , ICategoryRepository categoryRepository
        , IProductRepository productRepository)
    {
        _context = context;
        Categories = categoryRepository;
        Products = productRepository;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
        
}