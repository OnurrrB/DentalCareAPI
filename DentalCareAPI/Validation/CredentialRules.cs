using FluentValidation;

namespace DentalCareAPI.Validation;

public static class CredentialRules
{
    public const int MinimumPasswordLength = 8;

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().MaximumLength(254).EmailAddress();

    public static IRuleBuilderOptions<T, string> ValidNewPassword<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().MinimumLength(MinimumPasswordLength).MaximumLength(128);
}
