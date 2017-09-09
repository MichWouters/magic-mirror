using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Repos;
using MagicMirror.Entities.Traffic;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class TrafficService : ServiceBase<TrafficModel, TrafficEntity>
    {
        public TrafficService(SearchCriteria criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.HomeAddress)) throw new ArgumentNullException("A home address has to be provided");
            if (string.IsNullOrWhiteSpace(criteria.HomeCity)) throw new ArgumentNullException("A home town has to be provided");
            if (string.IsNullOrWhiteSpace(criteria.WorkAddress)) throw new ArgumentNullException("A destination address has to be provided");

            _criteria = criteria;
        }

        public override async Task<TrafficModel> GetModelAsync()
        {
            try
            {
                // Get entity from repository
                string homeAddress = $"{_criteria.HomeAddress} {_criteria.HomeCity}";
                _repo = new TrafficRepo(homeAddress, _criteria.WorkAddress);
                TrafficEntity entity = await _repo.GetEntityAsync();

                // Map entity to model
                TrafficModel model = MapEntityToModel(entity);

                // Calculate non-mappable values
                model = CalculateMappedValues(model);

                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Traffic Model", ex);
            }
        }

        protected override TrafficModel CalculateMappedValues(TrafficModel model)
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

        private TrafficModel MapEntityToModel(IEntity entity)
        {
            TrafficModel model = Mapper.Map<TrafficModel>(entity);
            return model;
        }
    }
}