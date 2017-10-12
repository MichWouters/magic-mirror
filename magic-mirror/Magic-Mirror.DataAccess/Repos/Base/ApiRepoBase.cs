using MagicMirror.DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public abstract class ApiRepoBase<T> : IApiRepo<T> where T : IEntity
    {
        protected virtual string _apiUrl { get; set; }
        protected virtual string _apiId { get; set; }
        protected virtual string _url { get; set; }

        /// <summary>
        /// Retrieve an Entity object from a Json Api source
        /// </summary>
        /// <returns></returns>
        public abstract Task<T> GetEntityAsync();

        /// <summary>
        /// Retrieve a Json object as string from an Api
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetJsonAsync()
        {
            try
            {
                var client = new HttpClient();
                string result = await client.GetStringAsync(_url);

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new ArgumentException("Unable to retrieve JSON from server", ex);
            }
        }

        /// <summary>
        /// Get an HttpResponse (with data) from an API. Throws catchable exception if no connection can be made
        /// </summary>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> GetHttpResponseFromApiAsync()
        {
            var client = new HttpClient();
            HttpResponseMessage _response = await client.GetAsync(_url);

            if (_response.IsSuccessStatusCode)
            {
                return _response;
            }
            else
            {
                var msg = $"{_response.StatusCode}: {_response.ReasonPhrase}";
                Debug.WriteLine(msg);
                throw new HttpRequestException(msg);
            }
        }

        /// <summary>
        /// Convert a string of Json into a C# Magic-Mirror Data Entity class
        /// </summary>
        /// <param name="json">Json string to convert to C# POCO</param>
        /// <returns></returns>
        protected T ConvertJsonToEntity(string json)
        {
            try
            {
                T entity = JsonConvert.DeserializeObject<T>(json);
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ArgumentException("Cannot convert Json to Entity", e);
            }
        }
    }
}