using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories;

public class AlarmRepository : IAlarmRepository
{
    private readonly AppDbContext _context;

    public AlarmRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Alarm?> GetAlarmByIdAsync(int id)
    {
        return await _context.Alarms.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IReadOnlyList<Alarm>> GetAlarmsByUserIdAsync(int userId)
    {
        return await _context.Alarms
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Time)
            .ToListAsync();
    }

    public async Task AddAlarmAsync(Alarm alarm)
    {
        _context.Alarms.Add(alarm);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAlarmAsync(Alarm alarm)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAlarmAsync(int id)
    {
        var deleted = await _context.Alarms.Where(a => a.Id == id).ExecuteDeleteAsync();
        return deleted > 0;
    }
}
