using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamp.Weather.Domain.Ef.Sql
{
    public class WeatherSqlContext : DbContext
    {
        /// <inheritdoc />
        public WeatherSqlContext(DbContextOptions<WeatherSqlContext>? options)
            : base(options)
        {
        }

        public virtual DbSet<DailyForecast>? DailyForecasts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("gamp-weather");

            modelBuilder
                .Entity<DailyForecast>()
                .HasData(GetForecasts()); //initia seed data
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private IEnumerable<DailyForecast> GetForecasts()
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