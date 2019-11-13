using Gamp.Weather.Abstractions;
using Gamp.Weather.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamp.Weather.Domain.Mongo
{
    public class MongoWeatherForecastService : WeatherForecastService
    {
        public override Task<IEnumerable<WeatherForecast>> GetForecasts()
        {
            throw new System.NotImplementedException();
        }
    }
}