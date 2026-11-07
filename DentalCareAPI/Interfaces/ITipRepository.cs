using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces;

public interface ITipRepository
{
    Task<Tip?> GetTipByIdAsync(int id);
    Task<IEnumerable<Tip>> GetAllTipsAsync();
}
