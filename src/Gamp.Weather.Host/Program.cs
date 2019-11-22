using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Reflection;
using AppHost = Microsoft.Extensions.Hosting.Host;

namespace Gamp.Weather.Host
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            //for a list of things CreateDefaultBuilder does see:
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-3.0
            AppHost
            .CreateDefaultBuilder(args)
            .UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                //default application name is assembly name, override to friendlier name
                hostingContext.HostingEnvironment.ApplicationName = "Gamp.Weather";

                // CreateDefaultBuilder adds configuration in this order:
                //  appsettings.json.
                //  appsettings.{ Environment}.json.
                //  Secret Manager when the app runs in the Development environment using the entry assembly.
                //  Environment variables.
                //  Command - line arguments.
                if (File.Exists($"{hostingContext.HostingEnvironment.ContentRootPath}\\appsettings.User.json"))
                {
                    // Add a user file (excluded from source) after all other sources to allow developers to
                    // override settings locally without playing tug-of-war with tracked source config files
                    config.AddJsonFile("appsettings.User.json", true);
                }
            })
            .ConfigureServices((hostContext, services) =>
             {
                 // Configure options for kestrel self-hosting from IConfiguration
                 // Note: This will override local launchSettings.json values
                 services.Configure<KestrelServerOptions>(
                     hostContext.Configuration.GetSection("Kestrel"));

                 var mode = hostContext.Configuration.GetValue("WeatherStore", "Sql");

                 // Add a Worker Service to run the auto-migrator
                 // See the docs for more informaion on Worker Services
                 // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.0&tabs=visual-studio

                 switch (mode.ToLowerInvariant())
                 {
                     case "sql":
                         services.AddWeatherDbContextMigrator(
                         hostContext.Configuration.GetConnectionString("WeatherSqlContext"));
                         break;

                     case "mongo":
                         services.AddWeatherMongoMigrator(hostContext.Configuration.GetConnectionString("WeatherMongoDb"));
                         break;

                     default:
                         //no migrator for memory or others
                         break;
                 }
             })
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                //get serilog settings from IConfiguration
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}