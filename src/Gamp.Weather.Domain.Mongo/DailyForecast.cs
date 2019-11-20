using Gamp.Weather.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Gamp.Weather.Domain.Mongo
{

    public class DailyForecast
    {
        private DateTimeOffset _day;

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Date
        {
            get => _day;
            set => _day = new DateTimeOffset(value.Date);
        }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public static implicit operator WeatherForecast(DailyForecast forecast)
        {
            return new WeatherForecast
            {
                Date = forecast.Date,
                TemperatureC = forecast.TemperatureC,
                Summary = forecast.Summary
            };
        }

        public static implicit operator DailyForecast(WeatherForecast forecast)
        {
            return new DailyForecast
            {
                Date = forecast.Date,
                TemperatureC = forecast.TemperatureC,
                Summary = forecast.Summary
            };
        }
    }
}