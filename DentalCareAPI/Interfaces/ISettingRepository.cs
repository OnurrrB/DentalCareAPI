using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces;

public interface ISettingRepository
{
    Task<Setting?> GetSettingByUserIdAsync(int userId);
    Task UpdateSettingAsync(Setting setting);
}
