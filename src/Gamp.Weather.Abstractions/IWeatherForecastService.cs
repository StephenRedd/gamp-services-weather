using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Abstractions
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}