namespace MagicMirror.Business.Models
{
    public class WeatherModel : Model
    {
        public string Location { get; set; }
        public string Icon { get; set; }
        public double TemperatureCelsius { get; set; }
        public double TemperatureKelvin { get; set; }
        public double TemperatureFahrenheit { get; set; }
        public double Temperature { get; set; }
        public string WeatherType { get; set; }
        public string SunRise { get; set; }
        public int SunRiseMilliseconds { get; set; }
        public string SunSet { get; set; }
        public int SunSetMilliSeconds { get; set; }
    }
}