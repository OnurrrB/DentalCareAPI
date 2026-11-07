using DentalCareAPI.DTOs;
using FluentValidation;

namespace DentalCareAPI.Validation;

public class BrushLogCreateDtoValidator : AbstractValidator<BrushLogCreateDto>
{
    private const int MaximumDurationInSeconds = 1800;
    private static readonly TimeSpan AllowedClockDrift = TimeSpan.FromMinutes(5);

    public BrushLogCreateDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(b => b.BrushedAt)
            .NotEmpty()
            .Must(brushedAt => brushedAt <= timeProvider.GetUtcNow() + AllowedClockDrift)
            .WithMessage("'Brushed At' cannot be in the future.");
        RuleFor(b => b.DurationInSeconds).InclusiveBetween(1, MaximumDurationInSeconds);
        RuleFor(b => b.Notes).MaximumLength(500);
    }
}
