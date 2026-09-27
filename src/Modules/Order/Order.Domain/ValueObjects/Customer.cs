using Order.Domain.Constants;

namespace Order.Domain.ValueObjects;

public sealed record Customer
{
    public Guid Id { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    private Customer() { }

    private Customer(Guid id, string phoneNumber, string name, string email, string address)
    {
        Id = id;
        PhoneNumber = phoneNumber;
        Name = name;
        Email = email;
        Address = address;
    }

    public static Customer Of(Guid id, string phoneNumber, string name, string email, string address)
    {
        if (id == Guid.Empty)
            throw new DomainException(OrderErrorCode.CustomerIdIsRequired);

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException(OrderErrorCode.CustomerPhoneNumberIsRequired);

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(OrderErrorCode.CustomerNameIsRequired);

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException(OrderErrorCode.CustomerEmailIsRequired);

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException(OrderErrorCode.CustomerAddressIsRequired);

        return new Customer(
            id,
            phoneNumber.Trim(),
            name.Trim(),
            email.Trim().ToLowerInvariant(),
            address.Trim());
    }
}