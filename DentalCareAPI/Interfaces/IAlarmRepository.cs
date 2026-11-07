using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces;

public interface IAlarmRepository
{
    Task<Alarm?> GetAlarmByIdAsync(int id);
    Task<IReadOnlyList<Alarm>> GetAlarmsByUserIdAsync(int userId);
    Task AddAlarmAsync(Alarm alarm);
    Task UpdateAlarmAsync(Alarm alarm);
    Task<bool> DeleteAlarmAsync(int id);
}
