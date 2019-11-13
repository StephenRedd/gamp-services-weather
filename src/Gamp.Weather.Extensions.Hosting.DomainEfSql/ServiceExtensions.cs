using Gamp.Weather.Domain.Ef.Sql;
using Gamp.Weather.Extensions.Hosting.DomainEfSql;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void AddWeatherDbContextMigrator(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<WeatherContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddHostedService<EfSqlMigratorWorker>();
        }


    }
}
