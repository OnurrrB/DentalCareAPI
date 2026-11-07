using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces
{
    public interface IBrushLogRepository
    {
        Task<BrushLog?> GetBrushLogByIdAsync(int id);
        Task<IEnumerable<BrushLog>> GetBrushLogsByUserIdAsync(int userId);
        Task AddBrushLogAsync(BrushLog brushLog);
        Task UpdateBrushLogAsync(BrushLog brushLog);
        Task DeleteBrushLogAsync(int id);
    }
}
