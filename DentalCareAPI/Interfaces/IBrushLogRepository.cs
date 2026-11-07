using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces;

public interface IBrushLogRepository
{
    Task<BrushLog?> GetBrushLogByIdAsync(int id, int userId);
    Task<(IReadOnlyList<BrushLog> Items, int TotalCount)> GetBrushLogsByUserIdAsync(int userId, int page, int pageSize);
    Task AddBrushLogAsync(BrushLog brushLog);
    Task<bool> DeleteBrushLogAsync(int id, int userId);
}
