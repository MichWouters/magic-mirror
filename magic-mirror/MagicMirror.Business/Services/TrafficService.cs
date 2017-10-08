using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class TrafficService : ServiceBase<TrafficModel, TrafficEntity>
    {
        public TrafficService(UserSettings criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.HomeAddress)) throw new ArgumentNullException("A home address has to be provided");
            if (string.IsNullOrWhiteSpace(criteria.HomeCity)) throw new ArgumentNullException("A home town has to be provided");
            if (string.IsNullOrWhiteSpace(criteria.WorkAddress)) throw new ArgumentNullException("A destination address has to be provided");

            _criteria = criteria;
            _repo = new TrafficRepo($"{_criteria.HomeAddress} {_criteria.HomeCity}", _criteria.WorkAddress);
        }

        public override async Task<TrafficModel> GetModelAsync()
        {
            try
            {
                // Get Entity
                var entity = await _repo.GetEntityAsync();

                // Map entity to model
                TrafficModel model = MapEntityToModel(entity);

                // Calculate non-mappable values
                model = CalculateUnMappableValues(model);

                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Traffic Model", ex);
            }
        }

        protected TrafficModel CalculateUnMappableValues(TrafficModel model)
        {
            model.Minutes = (model.Minutes / 60);
            model.TrafficDensity = CalculateTrafficDensity(model.NumberOfIncidents);
            model.HourOfArrival = DateTime.Now.AddMinutes(model.Minutes);

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

        public override TrafficModel GetOfflineModelAsync(string path)
        {
            throw new NotImplementedException();
        }

        public override void SaveOfflineModel(TrafficModel model, string path)
        {
            throw new NotImplementedException();
        }
    }
}