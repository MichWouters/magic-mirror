using MagicMirror.DataAccess.Configuration;
using MagicMirror.Entities.Traffic;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class TrafficRepo : ApiRepoBase<TrafficEntity>
    {
        public TrafficRepo(string start, string destination):base()
        {
            if (string.IsNullOrWhiteSpace(start)) throw new ArgumentNullException("Start address need to be provided");
            if (string.IsNullOrWhiteSpace(destination)) throw new ArgumentNullException("Destination address needs to be provided");

            SetApiParameters();
            
            _url = $"{_apiUrl}?origin={start}&destination={destination}&key={_apiId}";
        }

        public override async Task<TrafficEntity> GetEntityAsync()
        {
            try
            {
                HttpResponseMessage response = await GetHttpResponseFromApiAsync();

                string json = await response.Content.ReadAsStringAsync();
                TrafficEntity entity = ConvertJsonToEntity(json);
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
                string errorMessage = $"A connection with the traffic server could not be established.";
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