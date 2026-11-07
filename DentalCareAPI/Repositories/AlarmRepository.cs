using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories
{
    public class AlarmRepository : IAlarmRepository
    {
        private readonly AppDbContext _context;

        public AlarmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Alarm?> GetAlarmByIdAsync(int id)
        {
            return await _context.Alarms.FindAsync(id);
        }

        public async Task<IEnumerable<Alarm>> GetAlarmsByUserIdAsync(int userId)
        {
            return await _context.Alarms
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAlarmAsync(Alarm alarm)
        {
            await _context.Alarms.AddAsync(alarm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlarmAsync(Alarm alarm)
        {
            _context.Alarms.Update(alarm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlarmAsync(int id)
        {
            var alarm = await GetAlarmByIdAsync(id);
            if (alarm != null)
            {
                _context.Alarms.Remove(alarm);
                await _context.SaveChangesAsync();
            }
        }
    }
}
