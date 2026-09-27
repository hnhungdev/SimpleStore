using Order.Application.Features.Order.Commands.Validators;
using Order.Application.Features.Order.Dtos;
using Order.Application.Abstractions;
using Order.Domain.Constants;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.ValueObjects;

namespace Order.Application.Features.Order.Commands;

public sealed record CreateMyOrderCommand(Guid UserId, CreateOrderDto Dto) : ICommand<Guid>;

public sealed class CreateMyOrderCommandValidator
    : AbstractValidator<CreateMyOrderCommand>
{
    public CreateMyOrderCommandValidator()
    {
        RuleFor(x => x.Dto.Customer)
            .NotNull()
            .WithMessage(OrderErrorCode.BadRequest)
            .SetValidator(new CustomerValidator());

        RuleFor(x => x.Dto.PaymentMethod)
            .Must(IsValidPaymentMethod)
            .WithMessage(OrderErrorCode.InvalidPaymentMethod);
    }

    private static bool IsValidPaymentMethod(string? value)
    {
        return Enum.TryParse<PaymentMethod>(
                   value,
                   ignoreCase: true,
                   out var method)
               && Enum.IsDefined(method);
    }
}

public sealed class CreateMyOrderCommandHandler(
    IUnitOfWork unitOfWork,
    ICartModule cartModule,
    ICatalogModule catalogModule,
    IInventoryModule inventoryModule)
    : ICommandHandler<CreateMyOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateMyOrderCommand command, CancellationToken ct)
    {
        var dto = command.Dto;

        var paymentMethod = Enum.Parse<PaymentMethod>(dto.PaymentMethod, true);

        var customer = Customer.Of(
            command.UserId,
            dto.Customer.PhoneNumber,
            dto.Customer.Name,
            dto.Customer.Email,
            dto.Customer.Address);

        var cart = await cartModule.GetCartAsync(command.UserId, ct)
            ?? throw new NotFoundException(OrderErrorCode.CartNotFound);

        if (cart.Items.Count == 0)
            throw new ClientValidationException(OrderErrorCode.CartIsEmpty);

        var productIds = cart.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        var orderableProductsResponse =
            await catalogModule.GetOrderableProductsAsync(
                new OrderableProductsRequest(productIds), ct);

        var productLookup = orderableProductsResponse.Products
            .ToDictionary(x => x.Id);

        var missingProductIds = productIds
            .Where(id => !productLookup.ContainsKey(id))
            .ToList();

        if (missingProductIds.Count > 0)
            throw new ClientValidationException(OrderErrorCode.ProductNotFound, missingProductIds);

        var inventoryAvailabilityResponse = await inventoryModule.GetAvailabilityAsync(
            new InventoryAvailabilityRequest
            (
                cart.Items
                    .Select(x => new InventoryCheckItem(x.ProductId, x.Quantity))
                    .ToList()
            ), ct);

        var unavailableItems = inventoryAvailabilityResponse.Items
            .Where(x => x.RequestedQuantity > x.AvailableQuantity)
            .ToList();

        if (unavailableItems.Count > 0)
            throw new ClientValidationException(OrderErrorCode.InsufficientStock, unavailableItems);

        var orderItems = new List<(Product product, int quantity)>(cart.Items.Count);

        foreach (var cartItem in cart.Items)
        {
            var productInfo = productLookup[cartItem.ProductId];

            var product = Product.Of(
                productInfo.Id,
                productInfo.Name,
                productInfo.ThumbnailUrl,
                productInfo.Price);

            orderItems.Add((product, cartItem.Quantity));
        }

        var order = OrderEntity.Create(
            orderNo: OrderNo.Of(),
            customer: customer,
            paymentMethod: paymentMethod,
            items: orderItems,
            notes: dto.Notes);

        unitOfWork.Orders.Add(order);

        await unitOfWork.SaveChangesAsync(ct);

        return order.Id;
    }
}

