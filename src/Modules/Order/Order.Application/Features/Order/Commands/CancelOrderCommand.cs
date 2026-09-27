using Order.Application.Features.Order.Dtos;
using Order.Application.Abstractions;
using Order.Domain.Constants;
using MassTransit;

namespace Order.Application.Features.Order.Commands;

public sealed record CancelOrderCommand(Guid OrderId, CancelOrderDto Dto) : ICommand;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.OrderIdIsRequired);
        
        RuleFor(x => x.Dto.Reason)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CancelReasonIsRequired);
    }
}

public sealed class CancelOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand command, CancellationToken ct)
    {
        var order = await unitOfWork.Orders.GetByOrderIdAsync(command.OrderId, ct)
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