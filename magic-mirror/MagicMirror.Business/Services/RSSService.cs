using Acme.Generic;
using Acme.Generic.Extensions;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class RSSService : ServiceBase<RSSModel, RSSEntity>
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

        public override void SaveOfflineModel(RSSModel model, string path)
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
        
        private RSSModel GenerateOfflineModel()
        {
            return new RSSModel
            {
                items = new List<RSSItem>
                {
                    new RSSItem{ Title = "Aarde vergaan, Trump schuldig", Summary = "",Link=""},
                    new RSSItem{ Title = "Leven na de dood bewezen, kerkbezoek sterk gestegen", Summary = "",Link=""},
                    new RSSItem{ Title = "Grote terugroepactie Tedepi V9", Summary = "",Link=""}
                }
            }; 
        }
        
        
    }
}