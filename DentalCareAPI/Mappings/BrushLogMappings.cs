using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class BrushLogMappings
{
    public static BrushLog ToEntity(this BrushLogCreateDto dto, int userId) => new()
    {
        UserId = userId,
        BrushedAt = dto.BrushedAt,
        DurationInSeconds = dto.DurationInSeconds,
        Notes = dto.Notes,
    };

    public static BrushLogResponse ToResponse(this BrushLog log) =>
        new(log.Id, log.BrushedAt, log.DurationInSeconds, log.Notes);
}
