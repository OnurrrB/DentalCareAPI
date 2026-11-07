using DentalCareAPI.DTOs;
using FluentValidation;

namespace DentalCareAPI.Validation;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(u => u.Name).NotEmpty().MaximumLength(100);
        RuleFor(u => u.Age).InclusiveBetween(1, 120);
        RuleFor(u => u.Gender).IsInEnum();
    }
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        Include(new UserUpdateDtoValidator());
        RuleFor(u => u.Email).ValidEmail();
        RuleFor(u => u.Password).ValidNewPassword();
    }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.Email).NotEmpty();
        RuleFor(l => l.Password).NotEmpty();
    }
}
