using MagicMirror.DataAccess.Entities;
using System.Collections.Generic;

namespace MagicMirror.Entities.Traffic
{
    public class TrafficEntity : IEntity
    {
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }
        public List<Route> Routes { get; set; }
        public string Status { get; set; }
    }
}