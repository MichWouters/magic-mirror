using System.Collections.Generic;

namespace MagicMirror.Entities.Traffic
{
    public class Leg
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string EndAddress { get; set; }
        public EndLocation EndLocation { get; set; }
        public string StartAddress { get; set; }
        public StartLocation StartLocation { get; set; }
        public List<Step> Steps { get; set; }
        public List<object> TrafficSpeedEntry { get; set; }
        public List<object> ViaWaypoint { get; set; }
    }
}