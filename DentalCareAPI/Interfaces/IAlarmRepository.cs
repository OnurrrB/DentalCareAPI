using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces
{
    public interface IAlarmRepository
    {
        Task<Alarm?> GetAlarmByIdAsync(int id);
        Task<IEnumerable<Alarm>> GetAlarmsByUserIdAsync(int userId);
        Task AddAlarmAsync(Alarm alarm);
        Task UpdateAlarmAsync(Alarm alarm);
        Task DeleteAlarmAsync(int id);
    }
}
