using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Payment.Application.Features.Payment.Commands;
using Payment.Application.Features.Payment.Queries;
using Payment.Domain.Constants;

namespace Payment.Api.Endpoints;

public sealed class PaymentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/payments")
            .WithTags("Payments");

        group.MapGet("/gateways", GetAvailableGateways);

        group.MapGet("/vnpay-return", VnpayReturnAsync);

        group.MapGet("/momo-return", MomoReturnAsync);
    }

    private static async Task<IResult> GetAvailableGateways(
        ISender sender)
    {
        var gateways = await sender.Send(new GetAvailableGatewaysQuery(), default);
        return Results.Ok(gateways);
    }

    private static async Task<IResult> VnpayReturnAsync(
        ISender sender,
        HttpRequest request)
    {
        const string frontendOrigin = "http://localhost:5173";

        var txnRef = request.Query["vnp_TxnRef"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(txnRef))
        {
            throw new NotFoundException(PaymentErrorCode.PaymentTransactionNotFound);
        }

        var gatewayParams = request.Query.ToDictionary(
            x => x.Key,
            x => x.Value.ToString());

        var orderId = await sender.Send(new VerifyPaymentCommand
        (
            txnRef,
            gatewayParams
        ));

        return Results.Redirect($"{frontendOrigin}/orders/{orderId}");
    }

    private static async Task<IResult> MomoReturnAsync(
        ISender sender,
        HttpRequest request)
    {
        const string frontendOrigin = "http://localhost:5173";

        var txnRef = request.Query["orderId"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(txnRef))
        {
            throw new NotFoundException(PaymentErrorCode.PaymentTransactionNotFound);
        }

        var gatewayParams = request.Query
            .ToDictionary(
                x => x.Key,
                x => x.Value.ToString());

        var orderId = await sender.Send(new VerifyPaymentCommand
        (
            txnRef,
            gatewayParams
        ));

        return Results.Redirect($"{frontendOrigin}/orders/{orderId}");
    }
}