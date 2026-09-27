using System.Text.RegularExpressions;
using FluentValidation;

namespace BuildingBlocks.Application.Validators;

public static partial class CustomValidators
{
    /// <summary>
    /// Validates that a collection is not null and contains
    /// at least the specified number of elements.
    /// </summary>
    public static IRuleBuilderOptions<T, IList<TElement>> MustHave<T, TElement>(
        this IRuleBuilder<T, IList<TElement>> ruleBuilder,
        int count)
    {
        return ruleBuilder
            .NotNull()
            .Must(items => items.Count >= count)
            .WithMessage($"Must contain at least {count} item(s).");
    }

    /// <summary>
    /// Validates a Vietnamese phone number.
    ///
    /// Requirements:
    /// - Exactly 10 digits.
    /// - Starts with 03, 05, 07, 08 or 09.
    /// - Does not support +84 or formatted numbers.
    /// </summary>
    public static IRuleBuilderOptions<T, string> IsValidPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(BeAValidVietnamesePhoneNumber)
            .WithMessage("Invalid Vietnamese phone number.");
    }

    /// <summary>
    /// Validates a Vietnamese phone number with a custom error message.
    /// </summary>
    public static IRuleBuilderOptions<T, string> IsValidPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string errorMessage)
    {
        return ruleBuilder
            .Must(BeAValidVietnamesePhoneNumber)
            .WithMessage(errorMessage);
    }

    /// <summary>
    /// Validates a slug.
    ///
    /// Requirements:
    /// - Lowercase letters (a-z).
    /// - Numbers (0-9).
    /// - Single hyphens (-) between words.
    /// - Cannot start or end with a hyphen.
    /// - No consecutive hyphens.
    /// </summary>
    public static IRuleBuilderOptions<T, string> IsValidSlug<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(BeAValidSlug)
            .WithMessage(
                "Slug must contain only lowercase letters, numbers, and single hyphens.");
    }

    /// <summary>
    /// Validates a slug with a custom error message.
    /// </summary>
    public static IRuleBuilderOptions<T, string> IsValidSlug<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string errorMessage)
    {
        return ruleBuilder
            .Must(BeAValidSlug)
            .WithMessage(errorMessage);
    }

    private static bool BeAValidVietnamesePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        return VietnamesePhoneNumberRegex().IsMatch(phoneNumber);
    }

    private static bool BeAValidSlug(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        return SlugRegex().IsMatch(slug);
    }

    public static IRuleBuilderOptions<T, string?> MustBeAbsoluteUrl<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(url =>
            string.IsNullOrWhiteSpace(url) ||
            Uri.IsWellFormedUriString(url, UriKind.Absolute));
    }

    [GeneratedRegex(@"^(03|05|07|08|09)\d{8}$")]
    private static partial Regex VietnamesePhoneNumberRegex();

    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex SlugRegex();
}

