using Payment.Application.Abstractions;
using Payment.Application.Models;
using Payment.Domain.Enums;

namespace Payment.Infrastructure.Payments.Cod;

public sealed class CodPaymentService : IPaymentService
{
    public PaymentMethod Method => PaymentMethod.Cod;

    public PaymentRedirectBehavior RedirectBehavior => PaymentRedirectBehavior.None;

    public string Icon => "/images/payment-methods/cod.svg";

    public Task<CreatePaymentResult> CreatePaymentAsync(
        CreatePaymentRequest request,
        CancellationToken ct = default)
    {
        var gatewayTransactionId = $"COD_{Guid.NewGuid():N}";

        return Task.FromResult(new CreatePaymentResult(
            Success: true,
            PaymentUrl: "",
            GatewayTransactionId: gatewayTransactionId,
            RedirectBehavior: PaymentRedirectBehavior.None));
    }

    public Task<ConfirmPaymentResult> ConfirmPaymentAsync(
        ConfirmPaymentRequest request, 
        CancellationToken ct = default)
    {
        return Task.FromResult(new ConfirmPaymentResult(
            IsSuccess: true,
            OrderId: request.OrderId,
            GatewayTransactionId: request.GatewayTransactionId));
    }
}