namespace Payment.Application.Models;

public sealed record ConfirmPaymentResult(
    bool IsSuccess,
    Guid OrderId,
    string GatewayTransactionId,
    string? ErrorMessage = null);