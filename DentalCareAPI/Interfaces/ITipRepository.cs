using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces
{
    public interface ITipRepository
    {
        Task<Tip?> GetTipByIdAsync(int id);
        Task<IEnumerable<Tip>> GetAllTipsAsync();
        Task AddTipAsync(Tip tip);
        Task UpdateTipAsync(Tip tip);
        Task DeleteTipAsync(int id);
    }
}
