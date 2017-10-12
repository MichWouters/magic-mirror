﻿using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Entities;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class WeatherRepo : ApiRepoBase<WeatherEntity>
    {
        public WeatherRepo(string city) : base()
        {
            ValidateInputParameters(city);
            _url = $"{_apiUrl}/weather?q={city}&appid={_apiId}";
        }

        public override async Task<WeatherEntity> GetEntityAsync()
        {
            try
            {
                HttpResponseMessage response = await GetHttpResponseFromApiAsync();

                string json = await response.Content.ReadAsStringAsync();
                WeatherEntity entity = ConvertJsonToEntity(json);
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
                string errorMessage = $"A connection with the weather server could not be established.";
                throw new HttpRequestException(errorMessage, ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private void ValidateInputParameters(string city)
        {
            _apiUrl = DataAccessConfig.OpenWeatherApiUrl;
            _apiId = DataAccessConfig.OpenWeatherApiId;

            if (string.IsNullOrWhiteSpace(_apiUrl)) throw new ArgumentNullException("No Weather API Url provided");
            if (string.IsNullOrWhiteSpace(_apiId)) throw new ArgumentNullException("No Weather Api Id provided");
            if (string.IsNullOrWhiteSpace(city)) throw new ArgumentNullException("A home city has to be provided");
        }
    }
}