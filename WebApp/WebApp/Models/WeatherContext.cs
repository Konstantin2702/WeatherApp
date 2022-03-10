using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class WeatherContext:DbContext
    {
        public DbSet<WeatherInfo> WeatherInfos => Set<WeatherInfo>();
        public DbSet<WeatherCondition> WeatherConditions => Set<WeatherCondition>();
        public DbSet<SavedFiles> SavedFiles => Set<SavedFiles>();

        public WeatherContext(DbContextOptions<WeatherContext> options)
        : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }


    }
}
