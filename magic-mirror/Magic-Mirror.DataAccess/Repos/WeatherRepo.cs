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

        public WeatherRepo(string city)
        {
            // Defensive coding
            if (string.IsNullOrWhiteSpace(city)) throw new ArgumentNullException("A home city has to be provided");
            if (string.IsNullOrWhiteSpace(_apiUrl)) throw new ArgumentNullException("No API Url provided");
            if (string.IsNullOrWhiteSpace(_apiId)) throw new ArgumentNullException("No Api Id provided");

            _url = string.Format("{0}/weather?q={1}&appid={2}", _apiUrl, city, _apiId);
        }

        public async Task<WeatherEntity> GetJsonAsync()
        {
            try
            {
                var client = new HttpClient();
                HttpResponseMessage _response = await client.GetAsync(_url);

                if (_response.IsSuccessStatusCode)
                {
                    string json = await _response.Content.ReadAsStringAsync();
                    var entity = ConvertJsonToEntity(json);

                    return entity;
                }
                else
                {
                    throw new HttpRequestException("A connection with the server could not be established. Response: " + _response.StatusCode + ' ' + _response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not retrieve Json from server", e);
            }
        }

        private WeatherEntity ConvertJsonToEntity(string json)
        {
            try
            {
                WeatherEntity entity = JsonConvert.DeserializeObject<WeatherEntity>(json);
                return entity;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Cannot convert Json to Entity", e);
            }
        }

        public async Task<HttpResponseMessage> GetHttpResponseFromApi()
        {
            var client = new HttpClient();
            HttpResponseMessage _response = await client.GetAsync(_url);

            return _response;
        }
    }
}