using MagicMirror.DataAccess.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public interface IApiRepo<T> where T : IEntity
    {
        /// <summary>
        /// Retrieve an Entity object from a Json Api source
        /// </summary>
        /// <returns></returns>
        Task<T> GetEntityAsync();

        /// <summary>
        /// Retrieve a Json object from an Api
        /// </summary>
        /// <returns></returns>
        Task<string> GetJsonAsync();

        /// <summary>
        /// Get an HttpResponse (with data) from an API. Throws catchable exception if no connection can be made
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> GetHttpResponseFromApiAsync();
    }
}