using Order.Domain.Constants;
using Order.Domain.Enums;
using Order.Application.Features.Order.Dtos;
using Order.Application.Abstractions;
using MassTransit;

namespace Order.Application.Features.Order.Commands;

public sealed record UpdateOrderStatusCommand(Guid OrderId, UpdateOrderStatusDto Dto) : ICommand;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.OrderIdIsRequired);

        RuleFor(x => x.Dto.Status)
            .IsInEnum()
            .WithMessage(OrderErrorCode.InvalidOrderStatus);

        When(x => x.Dto.Status == OrderStatus.Canceled, () =>
        {
            RuleFor(x => x.Dto.Reason)
                .NotEmpty()
                .WithMessage(OrderErrorCode.CancelReasonIsRequired);
        });
    }
}

public sealed class UpdateOrderStatusCommandHandler(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand command, CancellationToken ct)
    {
        var order = await unitOfWork.Orders.GetByOrderIdAsync(command.OrderId, ct)
            ?? throw new NotFoundException(OrderErrorCode.OrderNotFound, command.OrderId);

        switch (command.Dto.Status)
        {
            case OrderStatus.Confirmed:
                order.Confirm();
                break;

            case OrderStatus.Shipping:
                order.Ship();
                break;

            case OrderStatus.Delivered:
                order.Deliver();
                
                if (order.PaymentMethod == PaymentMethod.Cod)
                {
                    await publishEndpoint.Publish(new OrderDeliveredIntegrationEvent
                    {
                        OrderId = order.Id
                    }, ct);
                }
                break;

            case OrderStatus.Canceled:
                order.Cancel(command.Dto.Reason!);
                break;
        }

        await unitOfWork.SaveChangesAsync(ct);
    }
}
