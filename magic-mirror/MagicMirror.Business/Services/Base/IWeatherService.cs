using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetModelAsync(string homeCity);

        WeatherModel GetOfflineModel(string path);

        void SaveOfflineModel(WeatherModel model, string path);
    }
}