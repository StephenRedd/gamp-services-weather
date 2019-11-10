using System.Diagnostics.CodeAnalysis;
using Gamp.Weather.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamp.Weather.Host
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public static void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWeatherApi(Configuration, env, "/weather");

        }
    }
}