using Payment.Application.Abstractions;
using Payment.Application.Models;
using Payment.Domain.Constants;
using Payment.Domain.Enums;

namespace Payment.Application.Features.Payment.Commands;

public sealed record VerifyPaymentCommand(
    string GatewayTransactionId,
    Dictionary<string, string> GatewayResponseParams
) : ICommand<Guid>;

public sealed class VerifyPaymentCommandValidator
    : AbstractValidator<VerifyPaymentCommand>
{
    public VerifyPaymentCommandValidator()
    {
        RuleFor(x => x.GatewayTransactionId)
            .NotEmpty()
            .WithMessage(
                PaymentErrorCode.GatewayTransactionIdIsRequired);

        RuleFor(x => x.GatewayResponseParams)
            .NotEmpty()
            .WithMessage(
                PaymentErrorCode.GatewayResponseParamsIsRequired);
    }
}

public sealed class VerifyPaymentHandler(
    IUnitOfWork unitOfWork,
    IPaymentServiceFactory paymentServiceFactory)
    : ICommandHandler<VerifyPaymentCommand, Guid>
{
    public async Task<Guid> Handle(
        VerifyPaymentCommand command,
        CancellationToken ct)
    {
        var payment = await unitOfWork.Payments
            .GetByGatewayTransactionIdAsync(command.GatewayTransactionId, ct)
            ?? throw new NotFoundException(PaymentErrorCode.PaymentTransactionNotFound);

        if (payment.Status != PaymentStatus.Pending)
        {
            return payment.OrderId;
        }

        var paymentService = paymentServiceFactory.GetService(payment.PaymentMethod);

        var confirmResult =
            await paymentService.ConfirmPaymentAsync(
                new ConfirmPaymentRequest(
                    OrderId: payment.OrderId,
                    GatewayTransactionId: command.GatewayTransactionId,
                    GatewayResponseParams: command.GatewayResponseParams), ct);

        if (!confirmResult.IsSuccess)
        {
            payment.MarkFailed(confirmResult.ErrorMessage);

            await unitOfWork.SaveChangesAsync(ct);

            return payment.OrderId;
        }

        payment.MarkSuccess();

        await unitOfWork.SaveChangesAsync(ct);

        return payment.OrderId;
    }
}