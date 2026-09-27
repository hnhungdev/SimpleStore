namespace Payment.Application.Models;

public sealed record ConfirmPaymentRequest(
    Guid OrderId,
    string GatewayTransactionId,
    Dictionary<string, string> GatewayResponseParams);