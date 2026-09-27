using Payment.Application.Abstractions;
using Payment.Application.Models;

namespace Payment.Application.Features.Payment.Queries;

public class GetAvailableGatewaysQuery : IQuery<IReadOnlyList<PaymentGatewayOption>>;

public class GetAvailableGatewaysHandler(IPaymentServiceFactory gatewayFactory) 
    : IQueryHandler<GetAvailableGatewaysQuery, IReadOnlyList<PaymentGatewayOption>>
{
    public Task<IReadOnlyList<PaymentGatewayOption>> Handle(GetAvailableGatewaysQuery query, CancellationToken ct)
    {
        var options = gatewayFactory.GetAvailableOptions();
        return Task.FromResult(options);
    }
}