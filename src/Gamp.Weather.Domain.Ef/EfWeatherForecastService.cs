using Gamp.Weather.Core;

namespace Gamp.Weather.Domain.Ef
{
    /// <summary> A service for accessing ef weather forecasts information. </summary>
    /// <remarks>
    ///     This class would inherit behavior that is common for the business domain that isn't
    ///     technology dependent; and override those members whose behavior may be dependent on EF
    ///     specific concepts but not concepts specific to a particular database technolgy. Furhter
    ///     derived types will override members that must be customized to a specific EF supported
    ///     database technolgy.
    /// </remarks>
    public abstract class EfWeatherForecastService : WeatherForecastService
    {
    }
}