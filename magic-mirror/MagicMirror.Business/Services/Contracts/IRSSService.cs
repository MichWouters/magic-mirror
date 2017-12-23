using System.Threading.Tasks;
using MagicMirror.Business.Models;

namespace MagicMirror.Business.Services
{
    public interface IRSSService
    {
        Task<RSSModel> GetModelAsync();
        RSSModel GetOfflineModel(string path);
        void SaveOfflineModel(RSSModel model, string path);
    }
}