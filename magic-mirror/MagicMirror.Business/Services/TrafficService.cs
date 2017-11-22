using Acme.Generic;
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
    public class TrafficService : ServiceBase<TrafficModel, TrafficEntity>, ITrafficService
    {
        private const string OFFLINEMODELNAME = "TrafficOfflineModel.json";

        public TrafficService()
        {
        }

        public async Task<TrafficModel> GetModelAsync(string homeAddress, string homeCity, string workAddress, DistanceUOM distanceUOM = DistanceUOM.Metric)
        {
            try
            {
                _repo = new TrafficRepo($"{homeAddress}, {homeCity}", workAddress);

                // Get Entity
                var entity = await _repo.GetEntityAsync();

                // Map entity to model
                TrafficModel model = MapEntityToModel(entity);

                // Calculate non-mappable values
                model = CalculateUnMappableValues(model, distanceUOM);

                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Traffic Model", ex);
            }
        }

        public TrafficModel GetOfflineModel(string path)
        {
            try
            {
                // Try reading Json object
                string json = FileWriter.ReadFromFile(path, OFFLINEMODELNAME);
                TrafficModel model = JsonConvert.DeserializeObject<TrafficModel>(json);

                return model;
            }
            catch (FileNotFoundException)
            {
                // Object does not exist. Create a new one
                TrafficModel offlineModel = GenerateOfflineModel();
                SaveOfflineModel(offlineModel, path);

                return GetOfflineModel(path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not read offline Weathermodel", e);
            }
        }

        protected TrafficModel CalculateUnMappableValues(TrafficModel model, DistanceUOM distanceUOM)
        {
            model.Minutes = (model.Minutes / 60);
            model.TrafficDensity = CalculateTrafficDensity(model.NumberOfIncidents);
            model.HourOfArrival = DateTime.Now.AddMinutes(model.Minutes);
            model.DistanceMiles = DistanceHelper.KiloMetersToMiles(model.DistanceKilometers);

            switch (distanceUOM)
            {
                case DistanceUOM.Imperial:
                    model.Distance = model.DistanceMiles + " miles";
                    break;

                case DistanceUOM.Metric:
                    model.Distance = model.DistanceKilometers + " km";
                    break;

                default:
                    model.Distance = model.DistanceKilometers + " km";
                    break;
            }

            return model;
        }

        private TrafficDensity CalculateTrafficDensity(int numberOfIncidents)
        {
            if (numberOfIncidents < 0) throw new ArgumentException("Number of incidents cannot be a negative number");

            TrafficDensity result;

            if (numberOfIncidents == 0)
            {
                result = TrafficDensity.Little;
            }
            else
            {
                if (numberOfIncidents <= 1)
                {
                    result = TrafficDensity.Light;
                }
                else if (numberOfIncidents <= 2)
                {
                    result = TrafficDensity.Medium;
                }
                else
                {
                    result = TrafficDensity.Heavy;
                }
            }
            return result;
        }

        public void SaveOfflineModel(TrafficModel model, string path)
        {
            try
            {
                string json = model.ToJson();
                FileWriter.WriteJsonToFile(json, OFFLINEMODELNAME, path);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not save offline Traffic Model", e);
            }
        }

        private TrafficModel GenerateOfflineModel()
        {
            var model = new TrafficModel
            {
                DistanceKilometers = 40.5,
                Minutes = (32 * 60),
                NumberOfIncidents = 2,
            };

            model = CalculateUnMappableValues(model, DistanceUOM.Metric);

            return model;
        }
    }
}