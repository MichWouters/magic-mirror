namespace MagicMirror.Business.Models.Weather
{
    public class WeatherModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public double DegreesCelsius { get; set; }
        public string WeatherType { get; set; }

        public string SunRise { get; set; }
        public string SunSet { get; set; }
    }
}
