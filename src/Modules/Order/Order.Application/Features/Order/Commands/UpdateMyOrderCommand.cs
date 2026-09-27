using Order.Domain.ValueObjects;
using Order.Domain.Constants;
using Order.Application.Abstractions;
using Order.Application.Features.Order.Commands.Validators;
using Order.Application.Features.Order.Dtos;

namespace Order.Application.Features.Order.Commands;

public sealed record UpdateMyOrderCommand(Guid UserId, Guid OrderId, UpdateOrderDto Dto) : ICommand;

public sealed class UpdateMyOrderCommandValidator : AbstractValidator<UpdateMyOrderCommand>
{
    public UpdateMyOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.OrderIdIsRequired);

        RuleFor(x => x.Dto.Customer)
            .NotNull()
            .WithMessage(OrderErrorCode.BadRequest)
            .SetValidator(new CustomerValidator());
    }
}

public sealed class UpdateMyOrderCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMyOrderCommand>
{
    public async Task Handle(UpdateMyOrderCommand command, CancellationToken ct)
    {
        var existingOrder = await unitOfWork.Orders.GetByOrderIdAndUserIdAsync(
                command.OrderId, command.UserId, ct)
            ?? throw new NotFoundException(OrderErrorCode.OrderNotFound, command.OrderId);

        var dto = command.Dto;

        var customer = Customer.Of(
            existingOrder.Customer.Id,
            dto.Customer.PhoneNumber,
            dto.Customer.Name,
            dto.Customer.Email,
            dto.Customer.Address);

        existingOrder.UpdateCustomerInfo(customer);

        await unitOfWork.SaveChangesAsync(ct);
    }
}