namespace DentalCareAPI.DTOs;

public record AlarmResponse(
    int Id,
    TimeOnly Time,
    bool RepeatMonday,
    bool RepeatTuesday,
    bool RepeatWednesday,
    bool RepeatThursday,
    bool RepeatFriday,
    bool RepeatSaturday,
    bool RepeatSunday,
    bool IsActive,
    string Label);
