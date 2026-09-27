namespace Payment.Application.Models;

public sealed record CreatePaymentRequest(
    Guid OrderId,
    Guid PaymentTransactionId,
    decimal Amount,
    string OrderDescription);