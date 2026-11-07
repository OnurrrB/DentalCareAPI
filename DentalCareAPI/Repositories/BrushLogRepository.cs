using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories
{
    public class BrushLogRepository : IBrushLogRepository
    {
        private readonly AppDbContext _context;

        public BrushLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BrushLog?> GetBrushLogByIdAsync(int id)
        {
            return await _context.BrushLogs.FindAsync(id);
        }

        public async Task<IEnumerable<BrushLog>> GetBrushLogsByUserIdAsync(int userId)
        {
            return await _context.BrushLogs
                .Where(bl => bl.UserId == userId)
                .ToListAsync();
        }

        public async Task AddBrushLogAsync(BrushLog brushLog)
        {
            await _context.BrushLogs.AddAsync(brushLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrushLogAsync(BrushLog brushLog)
        {
            _context.BrushLogs.Update(brushLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBrushLogAsync(int id)
        {
            var brushLog = await GetBrushLogByIdAsync(id);
            if (brushLog != null)
            {
                _context.BrushLogs.Remove(brushLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
