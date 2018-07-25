using System.Threading.Tasks;
using MagicMirror.Business.Models;

namespace MagicMirror.Business.Services
{
    public interface IRSSService
    {
        Task<RssModel> GetModelAsync();
    }
}