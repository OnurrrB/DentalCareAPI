using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;

        public SettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Setting?> GetSettingByUserIdAsync(int userId)
        {
            return await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task AddSettingAsync(Setting setting)
        {
            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSettingAsync(Setting setting)
        {
            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSettingAsync(int id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting != null)
            {
                _context.Settings.Remove(setting);
                await _context.SaveChangesAsync();
            }
        }
    }
}
