using Payment.Application.Abstractions;
using Payment.Domain.Repositories;

namespace Payment.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentDbContext _context;

    public IPaymentRepository Payments { get; }

    public UnitOfWork(PaymentDbContext context, 
    IPaymentRepository paymentRepository)
    {
        _context = context;
        Payments = paymentRepository;
    }
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
