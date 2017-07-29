using MagicMirror.Entities.Traffic;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Traffic
{
    public class TrafficRepo : IRepo<TrafficEntity>
    {
        private readonly string _apiId = DataAccessConfig.TrafficApiId;
        private readonly string _apiUrl = DataAccessConfig.TrafficApiUrl;

        private string _url;
        private SearchCriteria _criteria;

        public TrafficRepo(SearchCriteria criteria)
        {
            if (criteria == null)
                throw new NullReferenceException("Criteria cannot be empty");
            if (string.IsNullOrWhiteSpace(criteria.Start) || string.IsNullOrWhiteSpace(criteria.Destination))
                throw new ArgumentException("Start and destination addresses need to be provided");

            _criteria = criteria;
            _url = $"{_apiUrl}?origin={_criteria.Start}&destination={_criteria.Destination}&key={_apiId}";
        }

        public async Task<HttpResponseMessage> GetJsonResponseAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_url);

            return response;
        }

        public async Task<TrafficEntity> GetEntityAsync()
        {
            string json = await GetJsonAsync();

            TrafficEntity entity = JsonConvert.DeserializeObject<TrafficEntity>(json);
            return entity;
        }

        public async Task<string> GetJsonAsync()
        {
            HttpResponseMessage response = await GetJsonResponseAsync();

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

        public bool IsAbleToConnectToApi(HttpResponseMessage response)
        {
            return (response.IsSuccessStatusCode) ? true : false;
        }
    }
}