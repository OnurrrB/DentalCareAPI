namespace DentalCareAPI.DTOs;

public record BrushLogResponse(int Id, DateTimeOffset BrushedAt, int DurationInSeconds, string? Notes);
