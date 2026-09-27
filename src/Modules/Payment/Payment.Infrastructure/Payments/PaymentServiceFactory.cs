using Payment.Application.Abstractions;
using Payment.Application.Models;
using Payment.Domain.Enums;

namespace Payment.Infrastructure.Payments;

public sealed class PaymentServiceFactory : IPaymentServiceFactory
{
    private readonly Dictionary<PaymentMethod, IPaymentService> _services;

    public PaymentServiceFactory(IEnumerable<IPaymentService> services)
    {
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Method} - {service.GetType().Name}");
        }

        _services = services.ToDictionary(x => x.Method);
    }

    public IPaymentService GetService(PaymentMethod method)
    {
        if (!_services.TryGetValue(method, out var service))
            throw new InvalidOperationException(
                $"Payment method '{method}' is not registered.");

        return service;
    }

    public IReadOnlyList<PaymentGatewayOption> GetAvailableOptions()
    {
        return _services.Values
            .Select(x => new PaymentGatewayOption(
                x.Method.ToString(),
                x.Method.ToString(),
                x.RedirectBehavior,
                x.Icon))
            .ToList();
    }
}