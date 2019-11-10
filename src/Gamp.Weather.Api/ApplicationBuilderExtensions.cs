using System;
using System.Reflection;
using Gamp.Weather.Abstractions;
using Gamp.Weather.Api.Controllers;
using Gamp.Weather.Domain.Ef.Sql;
using Gamp.Weather.Domain.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiContrib.Core;

namespace Gamp.Weather.Api
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWeatherApi(
            this IApplicationBuilder rootApplication,
            IConfiguration configuration,
            IWebHostEnvironment env,
            PathString path)
        {
            return rootApplication.UseBranchWithServices(
                new[]
                {
                    new PathString(path)
                },
                services =>
                {
                    services.AddControllers(options =>
                        {
                            options.EnableEndpointRouting = true;
                        })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                        .AddApplicationPart(typeof(ForecastsController).GetTypeInfo().Assembly)
                        .AddControllersAsServices();

                    services.AddScoped(
                        typeof(IWeatherForecastService),
                        configuration
                            .GetValue("WeatherStore", "Sql")
                            .Equals("Mongo", StringComparison.InvariantCultureIgnoreCase)
                            ? typeof(EfSqlWeatherForecastService)
                            : typeof(MongoWeatherForecastService));
                },
                appBuilder =>
                {
                    if (env.IsDevelopment())
                    {
                        appBuilder.UseDeveloperExceptionPage();
                    }
                    appBuilder.UseHttpsRedirection();
                    appBuilder.UseRouting();
                    appBuilder.UseAuthorization();
                    appBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                });
        }
    }
}
