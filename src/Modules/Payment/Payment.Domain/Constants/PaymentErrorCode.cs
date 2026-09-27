namespace Payment.Domain.Constants;

public static class PaymentErrorCode
{
    public const string OrderIdIsRequired = "ORDER_ID_IS_REQUIRED";

    public const string UserIdIsRequired = "USER_ID_IS_REQUIRED";

    public const string ReturnUrlIsRequired = "RETURN_URL_IS_REQUIRED";

    public const string PaymentMethodIsRequired = "PAYMENT_METHOD_IS_REQUIRED";

    public const string GatewayTransactionIdIsRequired = "GATEWAY_TRANSACTION_ID_IS_REQUIRED";

    public const string GatewayResponseParamsIsRequired = "GATEWAY_RESPONSE_PARAMS_IS_REQUIRED";

    public const string AmountMustBeGreaterThanZero = "AMOUNT_MUST_BE_GREATER_THAN_ZERO";

    public const string GatewayAlreadyInitialized = "GATEWAY_ALREADY_INITIALIZED";

    public const string OnlyPendingTransactionCanBeMarkedAsSuccessful = "ONLY_PENDING_TRANSACTION_CAN_BE_MARKED_AS_SUCCESSFUL";

    public const string OnlyPendingTransactionCanBeMarkedAsFailed = "ONLY_PENDING_TRANSACTION_CAN_BE_MARKED_AS_FAILED";

    public const string OnlyPendingTransactionCanBeMarkedAsCancelled = "ONLY_PENDING_TRANSACTION_CAN_BE_MARKED_AS_CANCELLED";

    public const string OnlySuccessfulTransactionCanBeRefunded = "ONLY_SUCCESSFUL_TRANSACTION_CAN_BE_REFUNDED";

    public const string InvalidPaymentMethod = "INVALID_PAYMENT_METHOD";

    public const string PaymentTransactionNotFound = "PAYMENT_TRANSACTION_NOT_FOUND";

    public const string PaymentTransactionDoesNotBelongToOrder = "PAYMENT_TRANSACTION_DOES_NOT_BELONG_TO_ORDER";

    public const string PaymentCreationFailed = "PAYMENT_CREATION_FAILED";

    public const string InvalidStatusTransition = "INVALID_STATUS_TRANSITION";

    public const string PaymentNotFound = "PAYMENT_NOT_FOUND";
}
