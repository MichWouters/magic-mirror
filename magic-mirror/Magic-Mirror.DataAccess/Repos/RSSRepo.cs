using MagicMirror.DataAccess.Entities.Entities;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace MagicMirror.DataAccess.Repos
{
    public class RSSRepo : ApiRepoBase<RSSEntity>
    {
        private string _feed { get; set; }

        public RSSRepo() : base()
        {
            //TODO : config

            _feed = "http://www.standaard.be/rss/section/a30afc42-3737-4301-8f8a-5b6833855457";
            // _feed = "https://tvolen.wordpress.com/feed/";
            // _feed = "http://www.theonion.com/feeds/rss";
            // _feed = "http://www.hln.be/bizar/rss.xml";
        }

        public override async Task<RSSEntity> GetEntityAsync()
        {
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(_feed);
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    var feedReader = new RssFeedReader(reader);
                    var result = new RSSEntity();
                    result.Items = new List<RSSEntityItem>();

                    while (await feedReader.Read() && result.Items.Count <= 3)
                    {
                        switch (feedReader.ElementType)
                        {
                            // Read category
                            case SyndicationElementType.Category:
                                //DON't Care about categories ISyndicationCategory category = await feedReader.ReadCategory();
                                break;

                            // Read Image
                            case SyndicationElementType.Image:
                                //DON't Care about images ISyndicationImage image = await feedReader.ReadImage();
                                break;

                            // Read Item
                            case SyndicationElementType.Item:
                                ISyndicationItem item = await feedReader.ReadItem();
                                result.Items.Add(new RSSEntityItem { Title = item.Title, Summary = item.Description, Link = item.Links.FirstOrDefault().Uri.ToString() });
                                break;

                            // Read link
                            case SyndicationElementType.Link:
                                //DON't Care aboutISyndicationLink link = await feedReader.ReadLink();
                                break;

                            // Read Person
                            case SyndicationElementType.Person:
                                //DON't Care about ISyndicationPerson person = await feedReader.ReadPerson();
                                break;

                            // Read content
                            default:
                                //DON't Care about ISyndicationContent content = await feedReader.ReadContent();
                                break;
                        }
                    }

                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}