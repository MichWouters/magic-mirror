using MagicMirror.DataAccess.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public interface IApiRepo<T> where T : IEntity
    {
        Task<T> GetEntityAsync();

        Task<string> GetJsonAsync();

        Task<HttpResponseMessage> GetHttpResponseFromApiAsync();
    }
}