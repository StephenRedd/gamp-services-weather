using Gamp.Weather.Domain.Ef.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Weather.Extensions.Hosting.DomainEfSql
{
    public class EfSqlMigratorWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public EfSqlMigratorWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<WeatherContext>();

            await context.Database.MigrateAsync(cancellationToken);
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
