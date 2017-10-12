using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicMirror.Business.Models
{
    public class RSSModel : IModel
    {
        public List<RSSItem> items { get; set; }

        public string Summary
        {
            get
            {
                return items.Select(i => i.Title).Aggregate((a, b) => a + Environment.NewLine + b);
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