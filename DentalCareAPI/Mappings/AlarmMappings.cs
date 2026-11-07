using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class AlarmMappings
{
    public static Alarm ToEntity(this AlarmCreateDto dto, int userId) =>
        new Alarm { UserId = userId }.Apply(dto);

    public static Alarm Apply(this Alarm alarm, AlarmWriteDto dto)
    {
        alarm.Time = dto.Time;
        alarm.RepeatMonday = dto.RepeatMonday;
        alarm.RepeatTuesday = dto.RepeatTuesday;
        alarm.RepeatWednesday = dto.RepeatWednesday;
        alarm.RepeatThursday = dto.RepeatThursday;
        alarm.RepeatFriday = dto.RepeatFriday;
        alarm.RepeatSaturday = dto.RepeatSaturday;
        alarm.RepeatSunday = dto.RepeatSunday;
        alarm.IsActive = dto.IsActive;
        alarm.Label = dto.Label;
        return alarm;
    }

    public static AlarmResponse ToResponse(this Alarm alarm) => new(
        alarm.Id,
        alarm.Time,
        alarm.RepeatMonday,
        alarm.RepeatTuesday,
        alarm.RepeatWednesday,
        alarm.RepeatThursday,
        alarm.RepeatFriday,
        alarm.RepeatSaturday,
        alarm.RepeatSunday,
        alarm.IsActive,
        alarm.Label);
}
