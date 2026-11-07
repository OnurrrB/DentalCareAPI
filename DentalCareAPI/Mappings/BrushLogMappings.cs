using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class BrushLogMappings
{
    public static BrushLog ToEntity(this BrushLogCreateDto dto) => new()
    {
        UserId = dto.UserId,
        Date = dto.Date,
        DurationInSeconds = dto.DurationInSeconds,
        Notes = dto.Notes,
    };
}
