namespace Catalog.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    void Add(T entity);

    void Remove(T entity);
}