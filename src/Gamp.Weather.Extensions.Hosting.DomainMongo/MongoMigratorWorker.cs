using Gamp.Weather.Domain.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Weather.Extensions.Hosting.DomainMongo
{
    public class MongoMigratorWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public MongoMigratorWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var weatherService = scope.ServiceProvider.GetRequiredService<MongoWeatherForecastService>();
            return weatherService.Seed(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}