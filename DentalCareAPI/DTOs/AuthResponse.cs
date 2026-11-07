namespace DentalCareAPI.DTOs;

public record AuthResponse(string AccessToken, DateTimeOffset ExpiresAt);
