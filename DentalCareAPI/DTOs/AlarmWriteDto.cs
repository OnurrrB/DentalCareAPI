namespace DentalCareAPI.DTOs;

/// <summary>Fields a client may set on an alarm, shared by create and update.</summary>
public abstract class AlarmWriteDto
{
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
