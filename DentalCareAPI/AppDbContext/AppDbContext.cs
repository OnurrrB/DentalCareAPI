using Microsoft.EntityFrameworkCore;
using DentalCareAPI.Entities;

namespace DentalCareAPI.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<BrushLog> BrushLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tip> Tips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Setting: birebir
            modelBuilder.Entity<User>()
                .HasOne(u => u.Setting)
                .WithOne(s => s.User)
                .HasForeignKey<Setting>(s => s.UserId);

            // User - Alarms: bire-çok
            modelBuilder.Entity<User>()
                .HasMany(u => u.Alarms)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // User - BrushLogs: bire-çok
            modelBuilder.Entity<User>()
                .HasMany(u => u.BrushLogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);
        }
    }
}

