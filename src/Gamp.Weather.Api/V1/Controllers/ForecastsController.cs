using Gamp.Weather.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Api.V1_0
{
    /// <summary>
    ///     Weather Forecast endpoint
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class ForecastsController : V0_9.ForecastsController
    {
        /// <summary>
        ///     Initializes a new instance of the Weather Forecast controller
        /// </summary>
        /// <param name="logger">The MS logger instance to use for diagnostic logging.</param>
        /// <param name="forecastService">An instance of the Weather Forecast domain service.</param>
        public ForecastsController(
            ILogger<ForecastsController> logger,
            IWeatherForecastService forecastService) : base(logger, forecastService) { }

        /// <summary>
        ///     Gets the current forecast items
        /// </summary>
        /// <returns>A colleciton of weather forecasts.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public override Task<IEnumerable<WeatherForecast>> Get()
        {
            Logger.LogDebug("Getting forecasts");
            return base.Get(); ;
        }
    }
}