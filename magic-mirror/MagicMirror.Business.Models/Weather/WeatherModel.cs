using System;

namespace MagicMirror.Business.Models
{
    public class WeatherModel : Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public double DegreesCelsius { get; set; }
        public double DegreesKelvin { get; set; }
        public double DegreesFahrenheit { get; set; }
        public string WeatherType { get; set; }

        public DateTime SunRise { get; set; }
        public int SunRiseMilliseconds { get; set; }

        public DateTime SunSet { get; set; }
        public int SunSetMilliSeconds { get; set; }              
    }
}