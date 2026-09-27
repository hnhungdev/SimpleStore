using Order.Domain.Constants;
using Order.Application.Features.Order.Dtos;
using BuildingBlocks.Application.Validators;

namespace Order.Application.Features.Order.Commands.Validators;

public sealed class CustomerValidator : AbstractValidator<CustomerDto>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CustomerNameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CustomerEmailIsRequired)
            .EmailAddress()
            .WithMessage(OrderErrorCode.InvalidEmailAddress);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CustomerPhoneNumberIsRequired)
            .IsValidPhoneNumber()
            .WithMessage(OrderErrorCode.InvalidPhoneNumber);
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CustomerAddressIsRequired);
    }
}