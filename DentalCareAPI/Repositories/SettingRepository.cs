using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories;

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

    public async Task UpdateSettingAsync(Setting setting)
    {
        await _context.SaveChangesAsync();
    }
}
