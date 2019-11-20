using Gamp.Weather.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Core
{
    public abstract class WeatherForecastService : IWeatherForecastService
    {
        //any common behavior that is not technology dependent would go here

        public abstract Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}