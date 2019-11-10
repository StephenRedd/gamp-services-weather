using System;
using System.Diagnostics.CodeAnalysis;
using Gamp.Weather.Abstractions;
using Gamp.Weather.Domain.Ef.Sql;
using Gamp.Weather.Domain.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Gamp.Weather.Api
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped(
                typeof(IWeatherForecastService),
                Configuration
                    .GetValue("WeatherStore", "Sql")
                    .Equals("Mongo", StringComparison.InvariantCultureIgnoreCase)
                ? typeof(EfSqlWeatherForecastService)
                : typeof(MongoWeatherForecastService));
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}