using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Domain.Enums;
using Payment.Domain.Repositories;

namespace Payment.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _dbContext;
    private readonly DbSet<PaymentEntity> _dbSet;

    public PaymentRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<PaymentEntity>();
    }

    public void Add(PaymentEntity payment)
    {
        _dbSet.Add(payment);
    }

    public async Task<PaymentEntity?> GetByIdAsync(Guid paymentId, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Id == paymentId, ct);
    }

    public async Task<PaymentEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.OrderId == orderId, ct);
    }

    public async Task<PaymentEntity?> GetByGatewayTransactionIdAsync(string gatewayTransactionId, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.GatewayTransactionId == gatewayTransactionId, ct);
    }

    public async Task<List<PaymentEntity>> GetExpiredPendingPaymentsAsync(int take, DateTimeOffset now, CancellationToken ct)
    {
        return await _dbSet
            .Where(x =>
                x.Status == PaymentStatus.Pending &&
                x.ExpiredAt <= now)
            .OrderBy(x => x.ExpiredAt)
            .Take(take)
            .ToListAsync(ct);
    }
}