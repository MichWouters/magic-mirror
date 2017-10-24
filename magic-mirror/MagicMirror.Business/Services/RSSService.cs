using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class RSSService : ServiceBase<RSSModel, RSSEntity>
    {
        private const string OFFLINEMODELNAME = "RssOfflineModel.json";

        public RSSService()
        {
            _repo = new RSSRepo();
        }

        public override async Task<RSSModel> GetModelAsync()
        {
            try
            {
                // Get entity from Repository.
                var entity = await _repo.GetEntityAsync();

                // Map entity to model.
                var model = MapEntityToModel(entity);

                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve RSS Model", ex);
            }
        }

        public override RSSModel GetOfflineModel(string path)
        {
            try
            {
                // Try reading Json object
                /*     string json = FileWriter.ReadFromFile(path, OFFLINEMODELNAME);
                     var model = JsonConvert.DeserializeObject<RSSModel>(json);*/

                return GenerateOfflineModel();
            }
            catch (FileNotFoundException)
            {
                // Object does not exist. Create a new one
                var offlineModel = GenerateOfflineModel();
                SaveOfflineModel(offlineModel, path);

                return GetOfflineModel(path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not read offline RSS model", e);
            }
        }

        public override void SaveOfflineModel(RSSModel model, string path)
        {
            try
            {
                /*   string json = model.ToJson();
                   FileWriter.WriteJsonToFile(json, OFFLINEMODELNAME, path);*/
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not save offline RSS Model", e);
            }
        }

        private RSSModel GenerateOfflineModel()
        {
            string[] funnyPages = new string[]
               {
                    "Amerikaanse wapenfabrikanten betreuren overlijden van trouwe klant",
                    "Grondwettelijk Hof stelt voor 'Rich meet Beautiful open voor cougars, milfs en andere sugar mommy’s'",
                    "Man die na vijftien jaar uit coma ontwaakt, vraagt om meteen opnieuw in slaap gebracht te worden",
                    "Geheime liefdescorrespondentie Donald Trump en Kim Jong-Un gelekt",
                    "Leven na de dood bewezen, kerkbezoek sterk gestegen",
                    "Grote terugroepactie Tedepi V9",
                    "Slimme spiegel in ontwikkeling, Experts maken zich zorgen",
                    "Studie toont aan dat minder meer is. Min of meer",
                    "Man red kat uit boom, buren verrukt",
                    "Trein komt op tijd aan. Wetenschappers voor raadsel"
               };

            var model = new RSSModel();
            var items = new List<RSSItem>();

            foreach (string page in funnyPages)
            {
                items.Add(new RSSItem { Title = page });
            }

            model.items = items;
            return model;
        }
    }
}