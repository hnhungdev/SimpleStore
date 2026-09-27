using Payment.Application.Models;
using Payment.Domain.Enums;

namespace Payment.Application.Abstractions;

public interface IPaymentService
{
    /// <summary>
    /// Creates a payment request and returns the payment link/QR data and redirect behavior.
    /// </summary>
    Task<CreatePaymentResult> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default);

    /// <summary>
    /// Validates and confirms payment result from gateway callback/return URL.
    /// </summary>
    Task<ConfirmPaymentResult> ConfirmPaymentAsync(ConfirmPaymentRequest request, CancellationToken ct = default);

    /// <summary>
    /// The payment method this service handles.
    /// </summary>
    PaymentMethod Method { get; }

    /// <summary>
    /// How the client should handle this gateway's payment flow.
    /// </summary>
    PaymentRedirectBehavior RedirectBehavior { get; }

    /// <summary>
    /// Raw SVG icon markup rendered by frontend payment method buttons.
    /// </summary>
    string Icon { get; }
}
