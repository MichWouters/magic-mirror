using System.Collections.Generic;

namespace MagicMirror.Entities.Traffic
{
    public class GeocodedWaypoint
    {
        public string GeocoderStatus { get; set; }
        public string PlaceId { get; set; }
        public List<string> Types { get; set; }
    }
}