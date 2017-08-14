using MagicMirror.Business.Models;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Repos;
using MagicMirror.Entities.Traffic;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class TrafficService : ServiceBase, IService<TrafficModel>
    {
        private IRepo<TrafficEntity> _repo;

        public async Task<TrafficModel> GetModelAsync(SearchCriteria criteria)
        {
            // Defensive coding
            if (criteria == null) throw new ArgumentNullException("No search criteria provided", nameof(criteria));
            if (string.IsNullOrWhiteSpace(criteria.Start)) throw new ArgumentException("A home address has to be provided");
            if (string.IsNullOrWhiteSpace(criteria.Destination)) throw new ArgumentException("A destination address has to be provided");

            // Get entity from repository
            _repo = new TrafficRepo(criteria.Start, criteria.Destination);
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Map entity to model
            TrafficModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateMappedValues(model);

            return model;
        }

        public TrafficModel CalculateMappedValues(TrafficModel model)
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

        private TrafficModel MapEntityToModel(Entity entity)
        {
            TrafficModel model = Mapper.Map<TrafficModel>(entity);
            return model;
        }
    }
}