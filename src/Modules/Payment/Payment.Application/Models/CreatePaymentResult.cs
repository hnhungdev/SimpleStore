using Payment.Domain.Enums;

namespace Payment.Application.Models;

public sealed record CreatePaymentResult(
    bool Success,
    string PaymentUrl,
    string GatewayTransactionId,
    PaymentRedirectBehavior RedirectBehavior,
    DateTimeOffset? ExpiresAt = null,
    string? ErrorMessage = null);