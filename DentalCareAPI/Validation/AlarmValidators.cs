using DentalCareAPI.DTOs;
using FluentValidation;

namespace DentalCareAPI.Validation;

public class AlarmWriteDtoValidator : AbstractValidator<AlarmWriteDto>
{
    public AlarmWriteDtoValidator()
    {
        RuleFor(a => a.Label).NotEmpty().MaximumLength(100);
    }
}

public class AlarmCreateDtoValidator : AbstractValidator<AlarmCreateDto>
{
    public AlarmCreateDtoValidator()
    {
        Include(new AlarmWriteDtoValidator());
    }
}

public class AlarmUpdateDtoValidator : AbstractValidator<AlarmUpdateDto>
{
    public AlarmUpdateDtoValidator()
    {
        Include(new AlarmWriteDtoValidator());
    }
}
