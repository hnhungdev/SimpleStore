using Payment.Application.Models;
using Payment.Domain.Enums;

namespace Payment.Application.Abstractions;

public interface IPaymentServiceFactory
{
    IPaymentService GetService(PaymentMethod method);

    IReadOnlyList<PaymentGatewayOption> GetAvailableOptions();
}
