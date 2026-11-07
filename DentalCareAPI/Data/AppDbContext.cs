using DentalCareAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Alarm> Alarms => Set<Alarm>();
    public DbSet<BrushLog> BrushLogs => Set<BrushLog>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Tip> Tips => Set<Tip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.Email).HasMaxLength(254);
            user.HasIndex(u => u.Email).IsUnique();
            user.Property(u => u.PasswordHash).HasMaxLength(256);
            user.Property(u => u.Name).HasMaxLength(100);
            user.Property(u => u.Gender).HasConversion<string>().HasMaxLength(20);

            user.HasOne(u => u.Setting)
                .WithOne(s => s.User)
                .HasForeignKey<Setting>(s => s.UserId);

            user.HasMany(u => u.Alarms)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            user.HasMany(u => u.BrushLogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);
        });

        modelBuilder.Entity<Alarm>(alarm =>
        {
            alarm.Property(a => a.Label).HasMaxLength(100);
        });

        modelBuilder.Entity<BrushLog>(log =>
        {
            log.Property(b => b.Notes).HasMaxLength(500);
            // Serves the newest-first history query.
            log.HasIndex(b => new { b.UserId, b.BrushedAt }).IsDescending(false, true);
        });

        modelBuilder.Entity<Setting>(setting =>
        {
            setting.Property(s => s.Language).HasMaxLength(10);
        });

        modelBuilder.Entity<Tip>(tip =>
        {
            tip.Property(t => t.Title).HasMaxLength(150);
            tip.Property(t => t.Content).HasMaxLength(2000);
            tip.Property(t => t.ImageUrl).HasMaxLength(500);

            tip.HasData(
                new Tip
                {
                    Id = 1,
                    Title = "Brush twice a day",
                    Content = "Brush for two minutes in the morning and before bed with a fluoride toothpaste.",
                },
                new Tip
                {
                    Id = 2,
                    Title = "Clean between your teeth",
                    Content = "Use floss or an interdental brush once a day to reach where a toothbrush cannot.",
                },
                new Tip
                {
                    Id = 3,
                    Title = "Replace your toothbrush",
                    Content = "Change your toothbrush every three to four months, or sooner if the bristles are frayed.",
                },
                new Tip
                {
                    Id = 4,
                    Title = "Visit your dentist",
                    Content = "Regular check-ups catch problems early. Ask your dentist how often you should come in.",
                });
        });
    }
}
