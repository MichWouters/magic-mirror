using System;

namespace MagicMirror.Business.Models
{
    public class TrafficModel : Model
    {
        public string Distance { get; set; }

        public int Minutes { get; set; }

        public string MinutesText { get; set; }

        public int NumberOfIncidents { get; set; }

        public DateTime HourOfArrival { get; set; }

        public TrafficDensity TrafficDensity { get; set; }
    }

    public enum TrafficDensity
    {
        Few, Light, Medium, Heavy
    }
}