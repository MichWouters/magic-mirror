using System.Collections.Generic;

namespace MagicMirror.Entities.Traffic
{
    public class Route
    {
        public Bounds Bounds { get; set; }
        public string Copyrights { get; set; }
        public List<Leg> Legs { get; set; }
        public OverviewPolyline OverviewPolyline { get; set; }
        public string Summary { get; set; }
        public List<object> Warnings { get; set; }
        public List<object> WaypointOrder { get; set; }
    }
}