using Microsoft.EntityFrameworkCore;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Repositories;

namespace DentalCareAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(5161); // 5161 portunu tüm IP’lerde dinle
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();
            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<IBrushLogRepository, BrushLogRepository>();
            builder.Services.AddScoped<ITipRepository, TipRepository>();

            builder.Services.AddAutoMapper(typeof(Program));

            // CORS politikasý (mobil cihazlar için gerekli)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // CORS middleware
            app.UseCors("AllowAll");

            // HTTPS yönlendirme devre dýþý (mobil test için önerilir)
            // app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
