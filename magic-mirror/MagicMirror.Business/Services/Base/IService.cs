using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IApiService<T> where T : IModel
    {
        Task<T> GetModelAsync();

        T GetOfflineModel(string path);

        void SaveOfflineModel(T model, string path);
    }
}