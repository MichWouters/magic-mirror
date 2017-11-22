using System.Collections.Generic;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class RSSEntity : Entity
    {
        public List<RSSEntityItem> Items { get; set; }
    }

    public class RSSEntityItem
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}