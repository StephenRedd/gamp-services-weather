using Gamp.Weather.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamp.Weather.Domain.Ef
{
    /// <summary> A weather memory context. </summary>
    public class WeatherMemoryContext : DbContext
    {
        /// <inheritdoc/>
        public WeatherMemoryContext()
            : base(new DbContextOptionsBuilder<WeatherMemoryContext>()
                .UseInMemoryDatabase(databaseName: "WeatherMemoryDb")
                .Options)
        {
            this.Database.EnsureCreated();
        }

        /// <summary> Gets or sets the daily forecasts. </summary>
        /// <value> The daily forecasts. </value>
        public virtual DbSet<WeatherForecast>? DailyForecasts { get; set; }

        /// <summary>
        ///     <para>
        ///                     Override this method to configure the database (and other options) to be
        ///                     used for this context. This method is called for each instance of the
        ///                     context that is created. The base implementation does nothing.
        ///                 </para>
        ///     <para>
        ///                     In situations where an instance of
        ///                     <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or
        ///                     may not have been passed to the constructor, you can use
        ///     <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to
        ///     determine if the options have already been set, and skip some or all of the logic in
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///                 </para>
        /// </summary>
        /// <param name="optionsBuilder"> A builder used to create or modify options for this context.
        ///     Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention
        ///     from the entity types exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" />
        ///     properties on your derived context. The resulting model may be cached and re-used for
        ///     subsequent instances of your derived context.
        /// </summary>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        ///     then this method will not be run.
        /// </remarks>
        /// <param name="modelBuilder"> The builder being used to construct the model for this context.
        ///     Databases (and other extensions) typically define extension methods on this object that
        ///     allow you to configure aspects of the model that are specific to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<WeatherForecast>()
                .HasKey(f => f.Date);
            modelBuilder
                .Entity<WeatherForecast>()
                .HasData(GetForecasts());
        }

        /// <summary> The weather summaries. </summary>
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary> Gets the forecasts in this collection. </summary>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the forecasts in this collection.
        /// </returns>
        private IEnumerable<WeatherForecast> GetForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTimeOffset.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}