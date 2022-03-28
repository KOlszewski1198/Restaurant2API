
namespace Restaurant2API
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get(int num , int min, int max);
        IEnumerable<WeatherForecast> Get2();
        IEnumerable<WeatherForecast> Get3();
        IEnumerable<WeatherForecast> Get4(int num, int min, int max);
    }
}