using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentalCareAPI.Auth;

public class JwtOptions : IValidatableObject
{
    public const string SectionName = "Jwt";
    private const int MinimumKeyBytes = 32;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    /// <summary>Signing key. Comes from user secrets or the environment, never from appsettings.</summary>
    [Required]
    public string Key { get; set; } = string.Empty;

    [Range(1, 1440)]
    public int ExpiryMinutes { get; set; } = 60;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Encoding.UTF8.GetByteCount(Key) < MinimumKeyBytes)
        {
            yield return new ValidationResult(
                $"Jwt:Key must be at least {MinimumKeyBytes} bytes.", [nameof(Key)]);
        }
    }
}
