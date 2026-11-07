using DentalCareAPI.Entities;

namespace DentalCareAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
