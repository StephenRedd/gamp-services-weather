using Gamp.Weather.Api;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using WebApiContrib.Core;

namespace Gamp.Weather.Host
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Adds RFC7806 compliant error responses to APIs (https://tools.ietf.org/html/rfc7807)
            // This comes from a companion to the std problem details support in ASP.NET core which adds
            // middleware for exceptions that don't reach a controller -- such as 500 from filters, 404
            // from the router, etc.
            services
                .AddProblemDetails(options =>
                {
                    options.IncludeExceptionDetails = ctx => HostEnvironment.IsDevelopment();
                    options.Map<NotImplementedException>(ex =>
                        new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));
                    options.Map<HttpRequestException>(ex =>
                        new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));
                    options.Map<ArgumentOutOfRangeException>(ex =>
                        new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
                    options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
                });
            // Controllers (or Mvc/MvcCore) as well as API versioning must be added for problem details
            // middelware to work correctly even though the outer host isn't providing any aspnet core
            // endpoints (the split pipleline will).
            services.AddControllers();
            services.AddApiVersioning();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use problem details middleware
            app.UseProblemDetails();

            // MVC Contrib split pipeline middleware
            app.UseBranchWithServices(
                new[] { new PathString("/weather") },
                services => { services.AddWeatherApi(Configuration); },
                subApp => { subApp.UseWeatherApi(env); });
        }
    }
}