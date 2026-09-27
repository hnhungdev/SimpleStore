namespace Order.Domain.ValueObjects;

public sealed record OrderNo
{
    public string Value { get; }

    private OrderNo(string value) => Value = value;

    public static OrderNo Of()
    {
        var dateString = DateTimeOffset.UtcNow.ToString("yyyyMMdd");
        var sequenceString = Guid.NewGuid().ToString().Split("-").First().ToUpper();
        var orderNumber = $"ORD-{dateString}-{sequenceString}";
        return new OrderNo(orderNumber);
    }

    public static OrderNo Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Order number cannot be empty.", nameof(value));

        return new OrderNo(value);
    }

    public override string ToString() => Value;
}