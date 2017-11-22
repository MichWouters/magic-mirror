using MagicMirror.Entities.Traffic;
using System.Collections.Generic;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class TrafficEntity : Entity
    {
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }
        public List<Route> Routes { get; set; }
        public string Status { get; set; }
    }
}