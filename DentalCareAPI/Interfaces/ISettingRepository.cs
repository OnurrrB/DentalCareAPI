using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces
{
    public interface ISettingRepository
    {
        Task<Setting?> GetSettingByUserIdAsync(int userId);
        Task AddSettingAsync(Setting setting);
        Task UpdateSettingAsync(Setting setting);
        Task DeleteSettingAsync(int id);
    }
}
