using DentalCareAPI.Auth;
using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DentalCareAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
[EnableRateLimiting(AuthServiceCollectionExtensions.CredentialsRateLimitPolicy)]
public class AuthController : ControllerBase
{
    private const string DefaultLanguage = "en";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly TimeProvider _timeProvider;

    public AuthController(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService,
        TimeProvider timeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _timeProvider = timeProvider;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterDto dto)
    {
        var email = NormalizeEmail(dto.Email);
        if (await _userRepository.GetUserByEmailAsync(email) is not null)
            return Problem(statusCode: StatusCodes.Status409Conflict, detail: "This email is already registered.");

        var user = new User
        {
            Email = email,
            CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
            Setting = new Setting { Language = DefaultLanguage },
        }.Apply(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _userRepository.AddUserAsync(user);

        return Created("/api/users/me", _tokenService.CreateToken(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
    {
        var user = await _userRepository.GetUserByEmailAsync(NormalizeEmail(dto.Email));
        if (user is null ||
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "The email or password is incorrect.");
        }

        return Ok(_tokenService.CreateToken(user));
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
