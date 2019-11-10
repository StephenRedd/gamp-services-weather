using System.Collections.Generic;

namespace Gamp.Weather.Abstractions
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}