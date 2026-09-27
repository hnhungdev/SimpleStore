using MassTransit;
using Order.Application.Abstractions;
using Order.Application.Features.Order.Dtos;
using Order.Domain.Constants;

namespace Order.Application.Features.Order.Commands;

public sealed record CancelMyOrderCommand(Guid UserId, Guid OrderId, CancelOrderDto Dto) : ICommand;

public sealed class CancelMyOrderCommandValidator : AbstractValidator<CancelMyOrderCommand>
{
    public CancelMyOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.UserIdIsRequired);
            
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.OrderIdIsRequired);

        RuleFor(x => x.Dto.Reason)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CancelReasonIsRequired);
    }
}

public sealed class CancelMyOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<CancelMyOrderCommand>
{
    public async Task Handle(CancelMyOrderCommand command, CancellationToken ct)
    {
        var order = await unitOfWork.Orders.GetByOrderIdAndUserIdAsync(command.OrderId, command.UserId, ct)
            ?? throw new NotFoundException(OrderErrorCode.OrderNotFound, command.OrderId);

        order.Cancel(command.Dto.Reason);

        var integrationEvent = new OrderCancelledIntegrationEvent
        {
            OrderId = order.Id,
            Reason = command.Dto.Reason
        };

        await publishEndpoint.Publish(integrationEvent, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }
}