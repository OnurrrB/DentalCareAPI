using Microsoft.IdentityModel.JsonWebTokens;

namespace DentalCareAPI.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int Id
    {
        get
        {
            var subject = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return int.TryParse(subject, out var id)
                ? id
                : throw new InvalidOperationException("The request has no authenticated user.");
        }
    }
}
