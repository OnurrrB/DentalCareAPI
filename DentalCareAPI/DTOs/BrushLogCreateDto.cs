namespace DentalCareAPI.DTOs;

public class BrushLogCreateDto
{
    public DateTimeOffset BrushedAt { get; set; }
    public int DurationInSeconds { get; set; }
    public string? Notes { get; set; }
}
