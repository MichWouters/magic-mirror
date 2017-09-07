using MagicMirror.DataAccess.Configuration;
using MagicMirror.Entities.Traffic;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class TrafficRepo : IRepo<TrafficEntity>
    {
        private readonly string _apiId = DataAccessConfig.TrafficApiId;
        private readonly string _apiUrl = DataAccessConfig.TrafficApiUrl;

        private readonly string _url;

        //Todo: Abstract class with override?
        public TrafficRepo(string start, string destination)
        {
            if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(destination))
                throw new ArgumentNullException("Start and destination addresses need to be provided");
            if (string.IsNullOrWhiteSpace(_apiUrl)) throw new ArgumentNullException("No API Url provided");
            if (string.IsNullOrWhiteSpace(_apiId)) throw new ArgumentNullException("No Api Id provided");

            _url = $"{_apiUrl}?origin={start}&destination={destination}&key={_apiId}";
        }

        public async Task<TrafficEntity> GetEntityAsync()
        {
            string json = await GetJsonAsync();

            TrafficEntity entity = JsonConvert.DeserializeObject<TrafficEntity>(json);
            return entity;
        }

        //Todo: Abstract class
        public async Task<HttpResponseMessage> GetHttpResponseMessageFromAPI()
        {
            var client = new HttpClient();
            HttpResponseMessage _response = await client.GetAsync(_url);

            return _response;
        }

        public async Task<string> GetJsonAsync()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_url);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new HttpRequestException("Unable to retrieve Json Data. StatusCode: " + response.StatusCode);
            }
        }
    }
}