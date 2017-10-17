using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class AddressRepo : ApiRepoBase<AddressEntity>
    {
        public AddressRepo(string latitude, string longitude) : base()
        {
            if (string.IsNullOrWhiteSpace(latitude)) throw new ArgumentNullException("Latitude need to be provided");
            if (string.IsNullOrWhiteSpace(longitude)) throw new ArgumentNullException("Longitude needs to be provided");

            latitude = latitude.ConvertCommaToDot();
            longitude = longitude.Replace(',', '.');

            SetApiParameters();

            _url = $"{_apiUrl}/geocode/json?latlng={latitude},{longitude}&key={_apiId}";
        }

        public override async Task<AddressEntity> GetEntityAsync()
        {
            try
            {
                HttpResponseMessage response = await GetHttpResponseFromApiAsync();

                string json = await response.Content.ReadAsStringAsync();
                AddressEntity entity = ConvertJsonToEntity(json);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<HttpResponseMessage> GetHttpResponseFromApiAsync()
        {
            try
            {
                HttpResponseMessage result = await base.GetHttpResponseFromApiAsync();
                return result;
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"A connection with the traffic server for address retrieval could not be established.";
                throw new HttpRequestException(errorMessage, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetApiParameters()
        {
            _apiUrl = DataAccessConfig.TrafficApiUrl;
            _apiId = DataAccessConfig.TrafficApiId;

            if (string.IsNullOrWhiteSpace(_apiUrl)) throw new ArgumentNullException("No Traffic API Url provided");
            if (string.IsNullOrWhiteSpace(_apiId)) throw new ArgumentNullException("No Traffic Api Id provided");
        }
    }
}