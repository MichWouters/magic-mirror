using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class AddressService : ServiceBase<AddressModel, AddressEntity>, IAddressService
    {
        public async Task<AddressModel> GetModelAsync(string latitude, string longitude)
        {
            try
            {
                _repo = new AddressRepo(latitude, longitude);
                var entity = await _repo.GetEntityAsync();
                AddressModel model = MapEntityToModel(entity);

                return model;
            }
            catch (AutoMapper.AutoMapperMappingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve Address Model", ex);
            }
        }
    }
}