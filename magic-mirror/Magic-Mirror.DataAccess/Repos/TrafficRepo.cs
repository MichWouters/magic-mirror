using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Entities;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class TrafficRepo : ApiRepoBase<TrafficEntity>
    {
        public TrafficRepo(string start, string destination) : base()
        {
            if (string.IsNullOrWhiteSpace(start)) throw new ArgumentNullException("Start address need to be provided");
            if (string.IsNullOrWhiteSpace(destination)) throw new ArgumentNullException("Destination address needs to be provided");

            SetApiParameters();

            _url = $"{_apiUrl}/directions/json?origin={start}&destination={destination}&key={_apiId}";
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                Debug.WriteLine(ex.Message);
                string errorMessage = $"A connection with the traffic server for traffic duration could not be established.";
                throw new HttpRequestException(errorMessage, ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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