using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories;

public class BrushLogRepository : IBrushLogRepository
{
    private readonly AppDbContext _context;

    public BrushLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BrushLog?> GetBrushLogByIdAsync(int id)
    {
        return await _context.BrushLogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<(IReadOnlyList<BrushLog> Items, int TotalCount)> GetBrushLogsByUserIdAsync(
        int userId, int page, int pageSize)
    {
        var query = _context.BrushLogs.AsNoTracking().Where(b => b.UserId == userId);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.BrushedAt)
            .ThenByDescending(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddBrushLogAsync(BrushLog brushLog)
    {
        _context.BrushLogs.Add(brushLog);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteBrushLogAsync(int id)
    {
        var deleted = await _context.BrushLogs.Where(b => b.Id == id).ExecuteDeleteAsync();
        return deleted > 0;
    }
}
