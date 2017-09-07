using MagicMirror.DataAccess.Entities;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public interface IRepo<T> where T : Entity
    {
        T GetEntityAsync();

        Task<HttpResponseMessage> GetHttpResponseFromApi();
    }
}