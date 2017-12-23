using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MagicMirror.Business.Models
{
    public class RSSModel : Model
    {
        public ObservableCollection<RSSItem> Items { get; set; }

        public string Summary
        {
            get
            {
                return Items.Select(i => i.Title).Aggregate((a, b) => a + Environment.NewLine + b);
            }
        }
    }

    public class RSSItem
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}