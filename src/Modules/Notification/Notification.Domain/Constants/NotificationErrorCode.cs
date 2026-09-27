namespace Notification.Domain.Constants;

public sealed class NotificationErrorCode
{
    public const string BadRequest = "BAD_REQUEST";

    public const string DecisionFlowIllegal = "DECISION_FLOW_ILLEGAL";

    public const string InvalidEmailAddress = "INVALID_EMAIL_ADDRESS";

    public const string InvalidPhoneNumber = "INVALID_PHONE_NUMBER";

    public const string IdIsRequired = "ID_IS_REQUIRED";

    public const string UserIdIsRequired = "USER_ID_IS_REQUIRED";

    public const string ChanelIsRequired = "CHANEL_IS_REQUIRED";

    public const string AddressIsRequired = "ADDRESS_IS_REQUIRED";

    public const string SubjectIsRequired = "SUBJECT_IS_REQUIRED";

    public const string BodyIsRequired = "BODY_IS_REQUIRED";

    public const string NotificationJobNotFound = "NOTIFICATION_JOB_NOT_FOUND";

    public const string NotificationJobAlreadyExistsForEvent = "NOTIFICATION_JOB_ALREADY_EXISTS_FOR_EVENT";

    public const string EventIdIsRequired = "EvenIdIsRequired";

    public const string TemplateKeyIsRequired = "TEMPLATE_KEY_IS_REQUIRED";

    public const string TemplateNotFound = "TEMPLATE_NOT_FOUND";

    public const string AtLeastOneRecipientIsRequired = "AT_LEAST_ONE_RECIPIENT_IS_REQUIRED";

    public const string MaxAttemptsMustBeGreaterThanZero = "MAX_ATTEMPTS_MUST_BE_GREATER_THAN_ZERO";

    public const string NotificationNotFound = "NOTIFICATION_NOT_FOUND";

    public const string RecipientIsRequired = "RECIPIENT_IS_REQUIRED";

    public const string MessageIsRequired = "MESSAGE_IS_REQUIRED";

    public const string TitleIsRequired = "TITLE_IS_REQUIRED";

}
