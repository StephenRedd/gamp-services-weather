using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Gamp.Weather.Domain.Ef.Sql.Tooling
{
    public class WeatherDesignTimeContextFactory : IDesignTimeDbContextFactory<WeatherSqlContext>
    {
        public WeatherSqlContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Tooling/ef-design-settings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<WeatherSqlContext>();
            var connectionString = configuration.GetConnectionString("WeatherContext");
            optionsBuilder.UseSqlServer(connectionString);
            return new WeatherSqlContext(optionsBuilder.Options);
        }
    }
}