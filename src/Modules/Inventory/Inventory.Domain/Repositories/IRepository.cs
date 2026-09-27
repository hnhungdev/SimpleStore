namespace Inventory.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    void Add(T entity);

    void AddRange(List<T> list);

    void Remove(T entity);
}
