using MagicMirror.Business.Models.Traffic;
using System;
using System.Collections.Generic;
using System.Text;
using MagicMirror.DataAccess.Entities;
using System.Threading.Tasks;
using MagicMirror.DataAccess;
using MagicMirror.Entities.Traffic;
using MagicMirror.DataAccess.Traffic;

namespace MagicMirror.Business.Services.Traffic
{
    public class TrafficService : ServiceBase, IService<TrafficModel>
    {
        private readonly IRepo<TrafficEntity> _repo;
        private readonly SearchCriteria _criteria;

        public TrafficService()
        {
            _criteria = new SearchCriteria
            {
                Start = "Heikant 51 3390 Houwaart",
                Destination = "Generaal ArmstrongWeg 1 Antwerpen"
            };

            _repo = new TrafficRepo(_criteria);
        }

        public TrafficModel CalculateMappedValues(TrafficModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<TrafficModel> GetModelAsync()
        {
            // Get entity from repository
            TrafficEntity entity = await _repo.GetEntityAsync();

            // Map entity to model
            TrafficModel model = MapEntityToModel(entity);

            // Calculate non-mappable values
            model = CalculateMappedValues(model);

            return model;
        }

        public TrafficModel MapEntityToModel(Entity entity)
        {
            TrafficModel model = _mapper.Map<TrafficModel>(entity);
            return model;
        }
    }
}
