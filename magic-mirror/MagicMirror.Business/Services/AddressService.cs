﻿using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Diagnostics;
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

            _repo = new AddressRepo(_latitude, _longitude);

        }

        public override async Task<AddressModel> GetModelAsync()
        {
            try
            {
                var entity = await _repo.GetEntityAsync();
                AddressModel model = MapEntityToModel(entity);

                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new ArgumentException("Unable to retrieve Address Model", ex);
            }
        }

        public override AddressModel GetOfflineModel(string path)
        {
            var am = new AddressModel
            {

            };
            return am;
        }

        public override void SaveOfflineModel(AddressModel model, string path)
        {
            Debug.WriteLine("AddressService.SaveOfflineModel not implemented but used");
            throw new NotImplementedException();
        }
    }
}