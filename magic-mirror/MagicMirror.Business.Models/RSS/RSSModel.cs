namespace MagicMirror.Business.Models
{
    public class RSSModel : IModel
    {
        public List<RSSItem> items { get; set; }
    }

    public class RSSItem
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}