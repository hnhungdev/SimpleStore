using Payment.Domain.Repositories;

namespace Payment.Application.Abstractions;

public interface IUnitOfWork
{
    IPaymentRepository Payments { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}