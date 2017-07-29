using MagicMirror.DataAccess.Entities.Weather;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Weather
{
    public class WeatherRepo : IRepo<WeatherEntity>
    {
        private readonly string _apiUrl = DataAccessConfig.OpenWeatherApiUrl;
        private readonly string _apiId = DataAccessConfig.OpenWeatherApiId;
        private string _url;

        private HttpResponseMessage _response;
        private SearchCriteria _criteria;

        public bool IsAbleToConnectToApi
        {
            get { return (_response.IsSuccessStatusCode) ? true : false; }
        }

        public WeatherRepo(SearchCriteria criteria)
        {
            if (criteria == null) throw new ArgumentNullException("No search criteria provided");
            if (string.IsNullOrWhiteSpace(criteria.City)) throw new ArgumentException("A city has to be provided");

            _criteria = criteria;
            _url = string.Format("{0}/weather?q={1}&appid={2}", _apiUrl, _criteria.City, _apiId);
        }

        public async Task<HttpResponseMessage> GetJsonResponseAsync()
        {
            HttpClient client = new HttpClient();
            _response = await client.GetAsync(_url);

            return _response;
        }

        public async Task<string> GetJsonAsync()
        {
            _response = await GetJsonResponseAsync();

            if (IsAbleToConnectToApi)
            {
                string result = await _response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new HttpRequestException("A connection with the server could not be established");
            }
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

        public static implicit operator Action(WeatherRepo v)
        {
            throw new NotImplementedException();
        }
    }
}