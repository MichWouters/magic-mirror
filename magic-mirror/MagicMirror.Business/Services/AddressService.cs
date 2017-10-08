using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class AddressService : ServiceBase<AddressModel, AddressEntity>
    {
        private string _latitude;
        private string _longitude;

        public AddressService(string latitude, string longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public override async Task<AddressModel> GetModelAsync()
        {
            try
            {
                var entity = await GetEntityAsync();
                AddressModel model = MapEntityToModel(entity);

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Address Model", ex);
            }
        }

        public override Task<AddressModel> GetOfflineModelAsync()
        {
            throw new NotImplementedException();
        }

        public override Task SaveOfflineModel(AddressModel model)
        {
            throw new NotImplementedException();
        }

        protected override async Task<AddressEntity> GetEntityAsync()
        {
            _repo = new AddressRepo(_latitude, _longitude);
            AddressEntity entity = await _repo.GetEntityAsync();

            return entity;
        }
    }
}