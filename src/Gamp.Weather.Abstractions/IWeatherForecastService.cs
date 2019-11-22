using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Abstractions
{
    /// <summary> Interface for weather forecast service. </summary>
    public interface IWeatherForecastService
    {
        /// <summary> Gets the forecasts. </summary>
        /// <returns> An asynchronous result that yields the forecasts. </returns>
        Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}