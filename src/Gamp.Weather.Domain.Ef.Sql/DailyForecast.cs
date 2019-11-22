using Gamp.Weather.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamp.Weather.Domain.Ef.Sql
{
    /// <summary> A daily forecast DB Entity. </summary>
    /// <remarks>
    ///     This is a custom type used only for persistence. It is castable to the domain type
    ///     WeatherForecast via c# implicit operators. This compatibility can also be done via
    ///     inheritance or though explicit type mapping or with automapper and similar.
    /// </remarks>
    [Table("Forecasts")]
    public class DailyForecast
    {
        private DateTimeOffset _day;

        [Key]
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