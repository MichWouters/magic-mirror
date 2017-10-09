using MagicMirror.DataAccess.Entities.Weather;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class RSSEntity : IEntity
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}