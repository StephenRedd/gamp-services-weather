using Gamp.Weather.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamp.Weather.Domain.Ef.Sql
{
    public class EfSqlWeatherForecastService : EfWeatherForecastService
    {
        protected WeatherSqlContext Context { get; }

        public EfSqlWeatherForecastService(WeatherSqlContext context)
        {
            Context = context;
        }

        public override async Task<IEnumerable<WeatherForecast>> GetForecasts()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            //TODO: warning CS8602 appearing here seems like an analyzer bug, not a real problem...
            return (await Context.DailyForecasts?.Cast<WeatherForecast>().ToArrayAsync()) ?? new WeatherForecast[] { };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}