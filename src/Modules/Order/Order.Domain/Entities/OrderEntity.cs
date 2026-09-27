using Order.Domain.Constants;
using Order.Domain.Enums;
using Order.Domain.Events;
using Order.Domain.ValueObjects;

namespace Order.Domain.Entities;

public sealed class OrderEntity : AggregateRoot
{
    private readonly List<OrderItemEntity> _orderItems = [];

    private readonly List<OrderStatusHistoryEntity> _statusHistories = [];

    public IReadOnlyCollection<OrderItemEntity> OrderItems => _orderItems;

    public IReadOnlyList<OrderStatusHistoryEntity> StatusHistories => _statusHistories;

    public Customer Customer { get; private set; } = default!;

    public OrderNo OrderNo { get; private set; } = default!;

    public OrderStatus OrderStatus { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public string? Notes { get; private set; }

    public decimal TotalPrice { get; private set; }

    public DateOnly OrderDate { get; private set; }

    private OrderEntity() { }

    public static OrderEntity Create(
        OrderNo orderNo,
        Customer customer,
        PaymentMethod paymentMethod,
        IEnumerable<(Product Product, int Quantity)> items,
        string? notes)
    {
        var itemList = items.ToList();

        if (itemList.Count == 0)
            throw new DomainException(OrderErrorCode.OrderItemsIsRequired);

        var order = new OrderEntity
        {
            Customer = customer,
            OrderNo = orderNo,
            OrderStatus = OrderStatus.Pending,
            PaymentMethod = paymentMethod,
            Notes = notes,
            OrderDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        foreach (var (product, quantity) in itemList)
            order.AddOrderItem(product, quantity);

        order.RecalculatePrice();

        order.AddDomainEvent(
            new OrderCreatedDomainEvent(
                OrderId: order.Id,
                UserId: order.Customer.Id,
                OrderItems: order._orderItems
                    .Select(x => new OrderItemDomainEvent(
                        x.Product.Id,
                        x.Quantity))
                    .ToList(),
                TotalPrice: order.TotalPrice,
                PaymentMethod: order.PaymentMethod.ToString()));

        return order;
    }

    private void AddOrderItem(Product product, int quantity)
    {
        if (_orderItems.Any(x => x.Product.Id == product.Id))
            throw new DomainException(OrderErrorCode.DuplicateOrderItem);

        _orderItems.Add(
            OrderItemEntity.Create(Id, product, quantity));
    }

    private void RecalculatePrice()
    {
        TotalPrice = _orderItems.Sum(x => x.LineTotal);
    }

    public void Confirm()
    {
        if (OrderStatus == OrderStatus.Confirmed)
            return;

        if (OrderStatus != OrderStatus.Pending)
            throw new DomainException(OrderErrorCode.OrderCannotBeConfirmed);

        ChangeOrderStatus(OrderStatus.Confirmed);

        AddDomainEvent(new OrderConfirmedDomainEvent(
            Id,
            OrderNo.Value,
            Customer.Id,
            Customer.Name,
            Customer.Email,
            TotalPrice,
            PaymentMethod,
            DateTimeOffset.UtcNow));
    }

    public void Ship()
    {
        if (OrderStatus == OrderStatus.Shipping)
            return;

        if (OrderStatus != OrderStatus.Confirmed)
            throw new DomainException(OrderErrorCode.OrderCannotBeShipped);

        ChangeOrderStatus(OrderStatus.Shipping);
    }

    public void Deliver()
    {
        if (OrderStatus == OrderStatus.Delivered)
            return;

        if (OrderStatus != OrderStatus.Shipping)
            throw new DomainException(OrderErrorCode.OrderCannotBeDelivered);

        ChangeOrderStatus(OrderStatus.Delivered);

        //AddDomainEvent(new OrderDeliveredDomainEvent(Id));
    }

    public void Cancel(string reason)
    {
        if (OrderStatus == OrderStatus.Canceled)
            return;

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException(OrderErrorCode.CancelReasonIsRequired);

        EnsureCancellable();

        ChangeOrderStatus(OrderStatus.Canceled, reason);

        //AddDomainEvent(new OrderCancelledDomainEvent(Id, reason));
    }

    public void UpdateCustomerInfo(Customer customer)
    {
        EnsureEditable();
        Customer = customer;
    }

    private void ChangeOrderStatus(OrderStatus newStatus, string? reason = null)
    {
        var oldStatus = OrderStatus;
        OrderStatus = newStatus;

        _statusHistories.Add(OrderStatusHistoryEntity.Create(
            Id, oldStatus, newStatus, reason));
    }

    private void EnsureEditable()
    {
        if (OrderStatus is not OrderStatus.Pending)
            throw new DomainException(OrderErrorCode.OrderIsNotEditable);
    }

    private void EnsureCancellable()
    {
        if (OrderStatus is OrderStatus.Shipping or OrderStatus.Delivered or OrderStatus.Canceled)
            throw new DomainException(OrderErrorCode.OrderCannotBeCanceled);
    }
}