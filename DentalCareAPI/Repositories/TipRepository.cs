using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalCareAPI.Repositories
{
    public class TipRepository : ITipRepository
    {
        private readonly AppDbContext.AppDbContext _context;

        public TipRepository(AppDbContext.AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tip> GetTipByIdAsync(int id)
        {
            return await _context.Tips.FindAsync(id);
        }

        public async Task<IEnumerable<Tip>> GetAllTipsAsync()
        {
            return await _context.Tips.ToListAsync();
        }

        public async Task AddTipAsync(Tip tip)
        {
            await _context.Tips.AddAsync(tip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipAsync(Tip tip)
        {
            _context.Tips.Update(tip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTipAsync(int id)
        {
            var tip = await GetTipByIdAsync(id);
            if (tip != null)
            {
                _context.Tips.Remove(tip);
                await _context.SaveChangesAsync();
            }
        }
    }
}
