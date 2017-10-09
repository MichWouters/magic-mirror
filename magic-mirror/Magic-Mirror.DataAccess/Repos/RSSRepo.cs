using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class RSSRepo : ApiRepoBase<RSSEntity>
    {
        private string _feed { get; set; }

        public RSSRepo() : base()
        {
            //TODO : config
            _feed = "";
        }

        public override async Task<List<RSSEntity>> GetEntityAsync()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(_feed))
                {
                    var feed = SyndicationFeed.Load(reader);
                    return feed.Items.OrderByDescending(i => i.PublishDate).Take(3).Select(
                        i => new RSSEntity()
                        {
                            Title = i.Title.Text,
                            Summary = "TODO",
                            Link = "www.google.com"

                        }).ToList();
                }
            }
            catch (Exception) { throw; }
        }
    }
}