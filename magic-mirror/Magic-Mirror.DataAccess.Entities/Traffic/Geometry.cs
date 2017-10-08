using MagicMirror.Entities.Traffic;

namespace MagicMirror.DataAccess.Entities.Traffic
{
    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Bounds bounds { get; set; }
    }
}