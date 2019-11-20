using Gamp.Weather.Abstractions;
using Gamp.Weather.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Weather.Domain.Mongo
{
    public class MongoWeatherForecastService : WeatherForecastService
    {
        public MongoWeatherForecastService(IMongoClient client)
        {
            Client = client;
        }

        private IMongoClient Client { get; }

        public override async Task<IEnumerable<WeatherForecast>> GetForecasts()
        {
            var database = Client.GetDatabase("Weather");
            var d =
            (
                await database
                    .GetCollection<DailyForecast>(nameof(DailyForecast))
                    .Find(Builders<DailyForecast>.Filter.Empty).ToListAsync()
            );
            //the .Cast<T>() method for linq to objects doesn't work with implicit operators like it does with the EF specific Cast<T> extensions
            return d.Select(f => (WeatherForecast)f).ToArray();
        }

        public async Task Seed(CancellationToken cancellationToken = default)
        {
            var database = Client.GetDatabase("Weather");
            var collection = database.GetCollection<DailyForecast>(nameof(DailyForecast));
            await collection.DeleteManyAsync(Builders<DailyForecast>.Filter.Empty);
            await collection.InsertManyAsync(GetSeedForecasts());
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private IEnumerable<DailyForecast> GetSeedForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new DailyForecast
            {
                Date = DateTimeOffset.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}