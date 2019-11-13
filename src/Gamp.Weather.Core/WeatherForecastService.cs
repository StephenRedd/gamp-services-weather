using System;
using System.Collections.Generic;
using System.Linq;
using Gamp.Weather.Abstractions;
using System.Threading.Tasks;

namespace Gamp.Weather.Core
{
    public abstract class WeatherForecastService : IWeatherForecastService
    {
        //any common behavior that is not technology dependent would go here

        public abstract Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}