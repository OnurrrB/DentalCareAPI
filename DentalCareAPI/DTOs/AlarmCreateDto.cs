namespace DentalCareAPI.DTOs;

public class AlarmCreateDto
{
    public int UserId { get; set; }
    public TimeOnly Time { get; set; }
    public bool RepeatMonday { get; set; }
    public bool RepeatTuesday { get; set; }
    public bool RepeatWednesday { get; set; }
    public bool RepeatThursday { get; set; }
    public bool RepeatFriday { get; set; }
    public bool RepeatSaturday { get; set; }
    public bool RepeatSunday { get; set; }
    public bool IsActive { get; set; }
    public string Label { get; set; } = string.Empty;
}
