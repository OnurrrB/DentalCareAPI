using DentalCareAPI.Data;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Repositories;

public class TipRepository : ITipRepository
{
    private readonly AppDbContext _context;

    public TipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tip?> GetTipByIdAsync(int id)
    {
        return await _context.Tips.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tip>> GetAllTipsAsync()
    {
        return await _context.Tips.AsNoTracking().OrderBy(t => t.Id).ToListAsync();
    }
}
