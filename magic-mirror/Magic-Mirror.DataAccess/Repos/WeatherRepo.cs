using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Weather;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class WeatherRepo : IRepo<WeatherEntity>
    {
        private readonly string _apiUrl = DataAccessConfig.OpenWeatherApiUrl;
        private readonly string _apiId = DataAccessConfig.OpenWeatherApiId;
        private readonly string _url;

        private HttpResponseMessage _response;

        public WeatherRepo(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException("A home city has to be provided");

            _url = string.Format("{0}/weather?q={1}&appid={2}", _apiUrl, city, _apiId);
        }

        public async Task<WeatherEntity> GetEntityAsync()
        {
            try
            {
                string json = await GetJsonAsync();
                WeatherEntity entity = JsonConvert.DeserializeObject<WeatherEntity>(json);
                return entity;
            }
            catch
            {
                throw new ArgumentException("Cannot convert Json to objects");
            }
        }

        public async Task<string> GetJsonAsync()
        {
            _response = await GetJsonResponseAsync();

            if (_response.IsSuccessStatusCode)
            {
                string result = await _response.Content.ReadAsStringAsync();
                return result;
            }
            throw new HttpRequestException("A connection with the server could not be established");
        }

        public async Task<HttpResponseMessage> GetJsonResponseAsync()
        {
            var client = new HttpClient();
            _response = await client.GetAsync(_url);

            return _response;
        }
    }
}