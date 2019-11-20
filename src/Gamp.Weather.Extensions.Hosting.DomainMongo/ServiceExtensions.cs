using Gamp.Weather.Domain.Mongo;
using Gamp.Weather.Extensions.Hosting.DomainMongo;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void AddWeatherMongoMigrator(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddTransient<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddTransient<MongoWeatherForecastService>();

            services.AddHostedService<MongoMigratorWorker>();
        }


    }
}
