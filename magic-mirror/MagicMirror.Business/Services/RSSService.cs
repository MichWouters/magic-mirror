using Acme.Generic;
using Acme.Generic.Extensions;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class RSSService : ServiceBase<WeatherModel, WeatherEntity>
    {
        private const string OFFLINEMODELNAME = "WeatherOfflineModel.json";

        public RSSService()
        {
            // Defensive coding
          //  if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
           // if (string.IsNullOrWhiteSpace(criteria.HomeCity)) throw new ArgumentException("A city has to be provided");

            //TODO Config RSS feed

            // Set up parameters
        //    _criteria = criteria;
            _repo = new RSSRepo();
        }

        public override async Task<WeatherModel> GetModelAsync()
        {
            try
            {
                // Get entity from Repository.
                var entities = await _repo.GetEntityAsync();

                // Map entity to model.
                RSSModel model = MapEntityToModel(entity);
                
                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Weather Model", ex);
            }
        }

        public override RSSModel MapEntityToModel(List<RSSEntity> entity)
        {
            var model = new RSSModel();
            model.Items = new List<Items>();
            model.Items = Mapper.Map<List<RSSItem>>(entity);
            return model;
        }
        public override RSSModel GetOfflineModelAsync(string path)
        {
            try
            {
                // Try reading Json object
                string json = FileWriter.ReadFromFile(path, OFFLINEMODELNAME);
                var model = JsonConvert.DeserializeObject<RSSModel>(json);

                return model;
            }
            catch (FileNotFoundException)
            {
                // Object does not exist. Create a new one
                var offlineModel = GenerateOfflineModel();
                SaveOfflineModel(offlineModel, path);

                return GetOfflineModelAsync(path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not read offline RSS model", e);
            }
        }

        public override void SaveOfflineModel(WeatherModel model, string path)
        {
            try
            {
                string json = model.ToJson();
                FileWriter.WriteJsonToFile(json, OFFLINEMODELNAME, path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not save offline RSS Model", e);
            }
        }

        private WeatherModel GenerateOfflineModel()
        {
            return new RSSModel
            {
                items = new List<RSSItem>
                {
                    new RSSItem{ Titel = "Aarde vergaan, Trump schuldig", Summary = "",Url=""},
                    new RSSItem{ Titel = "Leven na de dood bewezen", Summary = "",Url=""},
                    new RSSItem{ Titel = "Kerkbezoek sterk gestegen", Summary = "",Url=""}
                }
            }; 
        }
        
        
    }
}