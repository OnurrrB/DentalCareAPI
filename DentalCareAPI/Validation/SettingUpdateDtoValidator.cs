using DentalCareAPI.DTOs;
using FluentValidation;

namespace DentalCareAPI.Validation;

public class SettingUpdateDtoValidator : AbstractValidator<SettingUpdateDto>
{
    private static readonly string[] SupportedLanguages = ["en", "tr"];

    public SettingUpdateDtoValidator()
    {
        RuleFor(s => s.Language)
            .Must(language => SupportedLanguages.Contains(language))
            .WithMessage($"'Language' must be one of: {string.Join(", ", SupportedLanguages)}.");
    }
}
