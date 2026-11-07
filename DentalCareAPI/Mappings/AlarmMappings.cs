using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class AlarmMappings
{
    public static Alarm ToEntity(this AlarmCreateDto dto) => new()
    {
        UserId = dto.UserId,
        Time = dto.Time,
        RepeatMonday = dto.RepeatMonday,
        RepeatTuesday = dto.RepeatTuesday,
        RepeatWednesday = dto.RepeatWednesday,
        RepeatThursday = dto.RepeatThursday,
        RepeatFriday = dto.RepeatFriday,
        RepeatSaturday = dto.RepeatSaturday,
        RepeatSunday = dto.RepeatSunday,
        IsActive = dto.IsActive,
        Label = dto.Label,
    };
}
