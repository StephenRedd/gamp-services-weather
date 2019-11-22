using Gamp.Weather.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Core
{
    /// <summary> A service for accessing weather forecasts information. </summary>
    /// <remarks>
    ///     This base class would contain only domain specific logic that is agnostic towards a
    ///     specific data storage technolgy. Derived types will override members that need to be
    ///     customized for technology specific concerns.
    /// </remarks>
    public abstract class WeatherForecastService : IWeatherForecastService
    {
        /// <summary> Gets the forecasts. </summary>
        /// <returns> An asynchronous result that yields the forecasts. </returns>
        public abstract Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}