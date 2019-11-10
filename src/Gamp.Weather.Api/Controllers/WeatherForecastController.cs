using System.Collections.Generic;
using Gamp.Weather.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gamp.Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private ILogger<WeatherForecastController> Logger { get; }
        private IWeatherForecastService ForecastService { get; }


        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherForecastService forecastService)
        {
            Logger = logger;
            ForecastService = forecastService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Logger.LogDebug("Getting forecasts");
            return ForecastService.GetForecasts();
        }
    }
}