namespace Notification.Domain.Enums;

public enum NotificationType
{
    OrderCreated = 1,
    OrderConfirmed = 2,
    OrderShipping = 3,
    OrderDelivered = 4,
    PaymentSuccess = 5,
    PaymentFailed = 6,
    PaymentCreated =7,
}