using Payment.Domain.Entities;

namespace Payment.Domain.Repositories;

public interface IPaymentRepository
{
    void Add(PaymentEntity payment);

    Task<PaymentEntity?> GetByIdAsync(Guid paymentId, CancellationToken ct = default);

    Task<PaymentEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default);

    Task<PaymentEntity?> GetByGatewayTransactionIdAsync(string gatewayTransactionId, CancellationToken ct = default);

    Task<List<PaymentEntity>> GetExpiredPendingPaymentsAsync(int take, DateTimeOffset now, CancellationToken ct = default);
}