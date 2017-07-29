using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
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