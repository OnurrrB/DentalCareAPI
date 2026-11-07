namespace DentalCareAPI.DTOs;

public class BrushLogCreateDto
{
    public int UserId { get; set; }
    public DateTimeOffset BrushedAt { get; set; }
    public int DurationInSeconds { get; set; }
    public string? Notes { get; set; }
}
