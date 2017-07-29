using System;
using System.Threading.Tasks;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.DataAccess.Entities;

namespace MagicMirror.Business.Services.Traffic
{
    public class TrafficService : ServiceBase, IService<TrafficModel>
    {
        public TrafficModel CalculateMappedValues(TrafficModel model)
        {
            throw new NotImplementedException();
        }

        public Task<TrafficModel> GetModelAsync()
        {
            throw new NotImplementedException();
        }

        public TrafficModel MapEntityToModel(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
