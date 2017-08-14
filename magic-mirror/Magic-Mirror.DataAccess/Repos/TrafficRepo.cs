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

        public TrafficRepo(string start, string destination)
        {
            if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(destination))
                throw new ArgumentNullException("Start and destination addresses need to be provided");

            _url = $"{_apiUrl}?origin={start}&destination={destination}&key={_apiId}";
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

        public async Task<HttpResponseMessage> GetJsonResponseAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(_url);

                return response;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("An invalid url was provided", e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}