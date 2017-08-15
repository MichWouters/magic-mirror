using MagicMirror.Business.Models;

namespace MagicMirror.ConsoleCore
{
    public class MagicMirrorDto
    {
        public string UserName { get; set; }
        public double DegreesCelsius { get; set; }
        public string TravelTime { get; set; }
        public string WeatherType { get; set; }
        public TrafficDensity TrafficDensity { get; set; }
    }
}