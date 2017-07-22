using MagicMirror.DataAccess.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess
{
    public interface IRepo<T> where T : Entity
    {
        /// <summary>
        /// Returns whether or not communication can occur between our app and the server.
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> GetJsonResponseAsync();

        /// <summary>
        /// Retrieve a Json object asynchrously from the server.
        /// </summary>
        /// <returns></returns>
        Task<string> GetJsonAsync();

        /// <summary>
        /// Convert the Json object to a C# POCO.
        /// </summary>
        /// <returns>C# poco filled with Json data</returns>
        Task<T> GetEntityAsync();
    }
}
