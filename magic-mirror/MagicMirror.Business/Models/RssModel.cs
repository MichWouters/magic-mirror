using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.Business.Models
{
    public class RssModel : Model
    {
        public List<RssItem> Items { get; set; }

        public override void ConvertValues()
        {
            throw new NotImplementedException();
        }
    }

    public class RssItem
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}
