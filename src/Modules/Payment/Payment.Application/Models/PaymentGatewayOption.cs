using Payment.Domain.Enums;

namespace Payment.Application.Models;

public sealed record PaymentGatewayOption(
    string Method,
    string DisplayName,
    PaymentRedirectBehavior RedirectBehavior,
    string Icon);
