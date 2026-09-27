using Payment.Application.Abstractions;
using Payment.Application.Models;
using Payment.Domain.Constants;
using Payment.Domain.Entities;
using Payment.Domain.Enums;

namespace Payment.Application.Features.Payment.Command;

public sealed record CreatePaymentCommand(
    Guid OrderId,
    Guid UserId,
    decimal Amount,
    string PaymentMethod) : ICommand;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(PaymentErrorCode.OrderIdIsRequired);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(PaymentErrorCode.UserIdIsRequired);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(PaymentErrorCode.AmountMustBeGreaterThanZero);

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage(PaymentErrorCode.PaymentMethodIsRequired)
            .Must(x => Enum.TryParse<PaymentMethod>(x, true, out _))
            .WithMessage(PaymentErrorCode.InvalidPaymentMethod);
    }
}

public sealed record CreatePaymentCommandHandler(
    IPaymentServiceFactory paymentServiceFactory,
    IUnitOfWork unitOfWork) : ICommandHandler<CreatePaymentCommand>
{
    public async Task Handle(CreatePaymentCommand command,
        CancellationToken ct)
    {

        var method = Enum.Parse<PaymentMethod>(command.PaymentMethod, true);

        var payment = PaymentEntity.Create(
            command.OrderId,
            command.Amount,
            method);

        unitOfWork.Payments.Add(payment);

        var service = paymentServiceFactory.GetService(method);

        var result = await service.CreatePaymentAsync(new CreatePaymentRequest(
            command.OrderId,
            payment.Id,
            command.Amount,
            $"OrderId={command.OrderId}"));

        if (!result.Success)
        {
            payment.MarkInitializationFailed(result.ErrorMessage!);

            await unitOfWork.SaveChangesAsync(ct);

            throw new ClientValidationException(PaymentErrorCode.PaymentCreationFailed, result.ErrorMessage);
        }

        payment.InitializeGateway(
            result.GatewayTransactionId,
            result.PaymentUrl,
            command.UserId,
            result.ExpiresAt);

        await unitOfWork.SaveChangesAsync(ct);
    }
}



