namespace DentalCareAPI.Entities;

public class BrushLog
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTimeOffset BrushedAt { get; set; }
    public int DurationInSeconds { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
}
