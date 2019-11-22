using Gamp.Weather.Abstractions;
using Gamp.Weather.Domain.Ef;
using Gamp.Weather.Domain.Ef.Sql;
using Gamp.Weather.Domain.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Gamp.Weather.Api
{
    /// <summary>
    ///     Extensions for registering services and configuring the aspnet core application builder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds the services and features needed by the Weather API
        /// </summary>
        /// <param name="services">The services collection to configure.</param>
        /// <param name="configuration">The application's IConfiguation instance</param>
        /// <returns>The services collection.</returns>
        public static IServiceCollection AddWeatherApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        // By default, ASP.NET Core 3 no longer depends on Newtonsoft.JSON We have
                        // JSON support under the System.Text namespace
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                        // Null properties will be omitted entirely from the JSON, useful for client
                        // that map missing properties as null.
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    })
                    .AddApplicationPart(typeof(ApplicationBuilderExtensions).GetTypeInfo().Assembly) // assembly where the controllers live
                    .AddControllersAsServices(); // registers controllers with DI

            services.AddApiVersioning(options =>
            {
                // Version attributes are not required if controllers are in folders following namespace conventions
                // See the docs here: https://github.com/microsoft/aspnet-api-versioning/wiki/API-Version-Conventions#version-by-namespace-convention
                options.Conventions.Add(new VersionByNamespaceConvention());

                // Outputs list of supported API versions in response header (api-supported-versions)
                // See the docs here: https://github.com/microsoft/aspnet-api-versioning/wiki/Version-Discovery
                options.ReportApiVersions = true;
            });

            // Supports versioning in the API Explorer (used by OpenAPI doc generators and code tools)
            services.AddVersionedApiExplorer(options =>
            {
                // Use 'v' prefixed format, with required major and optional minor version: e.g. "v0.9", "1", "v1.1"
                // See the docs here: https://github.com/microsoft/aspnet-api-versioning/wiki/Version-Format
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // NSwag - Add openapi json document for each api version -- first registered is default in swagger UI
            services
                .AddOpenApiDocument(document =>
                {
                    document.DocumentName = "v1"; // Affects the URL for the OAS JSON file
                    document.ApiGroupNames = new[] { "v1" };
                    document.PostProcess = d =>
                    {
                        d.Info.Title = "Gamp Weather APIs";
                        d.Info.Description =
                                    "JSON API services for the Gamp Weather.";
                    };
                }).AddOpenApiDocument(document =>
                {
                    document.DocumentName = "v0.9"; // Affects the URL for the OAS JSON file
                    document.ApiGroupNames = new[] { "v0.9" };
                    document.PostProcess = d =>
                    {
                        d.Info.Title = "Gamp Weather APIs";
                        d.Info.Description =
                                    "JSON API services for the Gamp Weather.";
                    };
                });

            // TODO: Consider passing this into method rather than relying on host to have included it in IConfiguration
            // The host may not know implicitly to support these settings in it's configuration
            var mode = configuration.GetValue("WeatherStore", "Sql");
            var weatherServiceType = typeof(EfMemoryWeatherForecastService);
            switch (mode.ToLowerInvariant())
            {
                case "sql":
                    services.AddDbContext<WeatherSqlContext>(
                     options => options.UseSqlServer(
                         configuration.GetConnectionString("WeatherSqlContext")));
                    weatherServiceType = typeof(EfSqlWeatherForecastService);
                    break;

                case "mongo":
                    services.AddSingleton<IMongoClient>(
                    _ => new MongoClient(
                        configuration.GetConnectionString("WeatherMongoDb")));
                    weatherServiceType = typeof(MongoWeatherForecastService);
                    break;

                default:
                    //default ctor self-initializes the db options for this context
                    services.AddDbContext<WeatherMemoryContext>();
                    break;
            }
            services.AddScoped(typeof(IWeatherForecastService), weatherServiceType);

            return services;
        }

        /// <summary>
        ///     Configures the aspnet core application builder for the Weather API
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeatherApi(
           this IApplicationBuilder app,
           IWebHostEnvironment env)
        {
            // problem detail exception mappings
            app.UseExceptionHandler
            (
                  env.IsDevelopment()
                ? "/error-dev"
                : "/error"
            );

            app.UseHsts();

            //TODO: probably need to configure this based on settings -- some deployment models will want to use straight HTTP
            app.UseHttpsRedirection();

            // Enable OpenAPI document generation
            app.UseOpenApi(options => { options.Path = "/openapi/{documentName}/openapi.json"; });

            // Enable the Swagger UI at '/openapi'
            app.UseSwaggerUi3(options =>
            {
                options.Path = "/openapi";
                options.DocumentPath = "/openapi/{documentName}/openapi.json";
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}