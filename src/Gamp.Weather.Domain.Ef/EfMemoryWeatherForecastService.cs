using Gamp.Weather.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamp.Weather.Domain.Ef
{
    /// <summary>
    ///     An service for accessing weather forecasts information using an in-memory EF store.
    /// </summary>
    /// <remarks>
    ///     This service shows usage of a domain entity as the persistence entity -- no domain to
    ///     data-specific mapping.
    /// </remarks>
    public class EfMemoryWeatherForecastService : EfWeatherForecastService
    {
        /// <summary>
        ///     Initializes a new instance of the Gamp.Weather.Domain.Ef.EfMemoryWeatherForecastService
        ///     class.
        /// </summary>
        /// <param name="context"> The EF InMemory DB context. </param>
        public EfMemoryWeatherForecastService(WeatherMemoryContext context)
        {
            Context = context;
        }

        /// <summary> Gets the DB context </summary>
        /// <value> The context. </value>
        protected WeatherMemoryContext? Context { get; }

        /// <summary> Gets the forecasts. </summary>
        /// <returns> An asynchronous result that yields the forecasts. </returns>
        public override Task<IEnumerable<WeatherForecast>> GetForecasts()
        {
            return Task.FromResult<IEnumerable<WeatherForecast>>(Context?.DailyForecasts.ToArray() ?? new WeatherForecast[]{ });
        }
    }
}
