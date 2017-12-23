using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IRSSService
    {
        Task<RSSModel> GetModelAsync();

        RSSModel GetOfflineModel(string path);

        void SaveOfflineModel(RSSModel model, string path);
    }
}